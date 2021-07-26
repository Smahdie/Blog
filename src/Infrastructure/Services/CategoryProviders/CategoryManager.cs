using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.CategoryProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.CategoryProviders
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PanelAppSettings _appSettings;
        public CategoryManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<CategoryGridDto> Items, int TotalCount)> GetAllAsync(CategoryGridDto dto)
        {
            var query = _dbContext.Categories.Where(c => !c.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Name), c => c.Translations.Any(t=>t.Name.Contains(dto.Name.Trim())));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.ParentName), c => c.ParentId.HasValue && c.Parent.Translations.Any(t => t.Name.Contains(dto.ParentName.Trim())));
            query = query.WhereIf(dto.IsActive.HasValue, c => c.IsActive == dto.IsActive);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(category => new CategoryGridDto
            {
                Id = category.Id,
                IsActive = category.IsActive,
                Name = category.Translations.First(t=> t.Language == "fa").Name,
                ParentName = category.ParentId.HasValue? category.Parent.Translations.First(t => t.Language == "fa").Name : "---",
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(category.CreatedOn)
            }).ToListAsync();

            return (items, totalCount);
        }

        public Task<List<Select2ItemDto>> GetSelectListAsync(int? toExclude = null)
        {
            return _dbContext.Categories
                .Where(c => !c.Deleted && c.ParentId == null && c.Id != toExclude)
                .OrderByDescending(c => c.Id)
                .Select(c => new Select2ItemDto 
                { 
                    Id = c.Id.ToString(),
                    Text = c.Translations.First(t=>t.Language == "fa").Name
                })
                .ToListAsync();
        }

        public Task<List<JsTreeNode>> GetTreeAsync()
        {
            return _dbContext.Categories
               .Where(c => !c.Deleted && c.IsActive)
               .OrderByDescending(c => c.Id)
               .Select(c => new JsTreeNode
               {
                   Id = c.Id.ToString(),
                   Text = c.Translations.First(t => t.Language == "fa").Name,
                   Parent = c.ParentId.HasValue ? c.ParentId.ToString() : "#"
               })
               .ToListAsync();
        }

        public async Task<CategoryUpdateDto> GetAsync(int id)
        {
            var category = await _dbContext.Categories.Include(c=>c.Translations).Include(c=>c.Parent.Translations).FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
            if(category == null)
            {
                return null;
            }

            return new CategoryUpdateDto
            {
                Id = category.Id,
                IsActive = category.IsActive,
                ParentId = category.ParentId,
                ParentName = (category.ParentId.HasValue ? category.Parent.Translations.First(t => t.Language == "fa").Name : null),
                Translations = category.Translations.Select(t => new CategoryTranslationDto { Language = t.Language, Name = t.Name })
            };
        }
       
        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (category == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                category.Deleted = true;
                category.DeletedOn = DateTime.Now;
                _dbContext.Update(category);

                _dbContext.Content2Categories.RemoveRange(_dbContext.Content2Categories.Where(t => t.CategoryId == id));

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return DeleteResultDto.Successful(id.ToString());
            }
            catch
            {
                return DeleteResultDto.UnknownError(id.ToString());
            }
        }
      
        public async Task<ChangeStatusResultDto> ChangeStatusAsync(int id)
        {
            var propertyInfo = typeof(CategoryGridDto).GetProperty(nameof(CategoryGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var category = await _dbContext.Categories
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (category == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                category.IsActive = !category.IsActive;
                category.UpdatedOn = DateTime.Now;

                _dbContext.Update(category);
                await _dbContext.SaveChangesAsync();

                if (category.IsActive)
                {
                    return ChangeStatusResultDto.Successful(id.ToString(), booleanAttribute.TrueBadge, booleanAttribute.TrueText);
                }

                return ChangeStatusResultDto.Successful(id.ToString(), booleanAttribute.FalseBadge, booleanAttribute.FalseText);
            }
            catch
            {
                return ChangeStatusResultDto.UnknownError(id.ToString());
            }
        }
       
        public async Task<CommandResultDto> CreateAsync(CategoryCreateDto dto)
        {
            var translations = JsonSerializer.Deserialize<List<CategoryTranslation>>(dto.Names, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); 
            if(translations.Any(t=> string.IsNullOrWhiteSpace(t.Name)))
            {
                return CommandResultDto.InvalidModelState(new List<string> { "برای همه زبانها نام مشخص کنید." });
            }

            var category = new Category
            {
                ParentId = dto.ParentId,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.Now,
                Translations = translations
            };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return CommandResultDto.Successful();
        }

        public async Task<CommandResultDto> UpdateAsync(CategoryUpdateDto dto)
        {
            var translations = JsonSerializer.Deserialize<List<CategoryTranslation>>(dto.Names, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (translations.Any(t => string.IsNullOrWhiteSpace(t.Name)))
            {
                return CommandResultDto.InvalidModelState(new List<string> { "برای همه زبانها نام مشخص کنید." });
            }

            var category = await _dbContext.Categories
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == dto.Id && !c.Deleted);

            if (category == null)
            {
                return CommandResultDto.NotFound();
            }

            category.IsActive = dto.IsActive;
            category.ParentId = dto.ParentId;
            category.UpdatedOn = DateTime.Now;
            _dbContext.Update(category);

            var prevTranslations = _dbContext.CategoryTranslations.Where(t => t.CategoryId == category.Id);
            _dbContext.CategoryTranslations.RemoveRange(prevTranslations);


            translations = translations.Select(t => { t.CategoryId = category.Id; return t; }).ToList();
            await _dbContext.CategoryTranslations.AddRangeAsync(translations);

            await _dbContext.SaveChangesAsync();

            return CommandResultDto.Successful();
        }
    }
}
