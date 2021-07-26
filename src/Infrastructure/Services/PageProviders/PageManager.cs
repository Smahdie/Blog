using Core.Dtos.CommonDtos;
using Core.Dtos.PageDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.PageProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.PageProviders
{
    public class PageManager : IPageManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PanelAppSettings _appSettings;
        public PageManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<PageGridDto> Items, int TotalCount)> GetAllAsync(PageGridDto dto)
        {
            var query = _dbContext.Pages.Where(p => !p.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Title), c => c.Title.Contains(dto.Title.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Keyword), c => c.Keyword.Contains(dto.Keyword.Trim()));
            query = query.WhereIf(dto.IsActive.HasValue, c => c.IsActive == dto.IsActive);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(dto.UpdatedOn))
            {
                var updatedOnDateTime = DateTime.Parse(dto.UpdatedOn);
                query = query.Where(c => c.UpdatedOn.HasValue && c.UpdatedOn > updatedOnDateTime.Date && c.UpdatedOn < updatedOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(page => new PageGridDto
            {
                Id = page.Id,
                Title = page.Title,
                Keyword = page.Keyword,
                IsActive = page.IsActive,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(page.CreatedOn),
                UpdatedOn = (page.UpdatedOn != null ? PersianDateHelper.ConvertToLocalDateTime(page.UpdatedOn.Value) : "---")
            }).ToListAsync();

            return (items, totalCount);
        }
       
        public async Task<PageUpdateDto> GetAsync(int id)
        {
            var page = await _dbContext.Pages.FirstOrDefaultAsync(c => c.Id == id);
            if (page == null)
            {
                return null;
            }

            return new PageUpdateDto
            {
                Id = page.Id,
                Title = page.Title,
                Body = page.Body,
                Summary = page.Summary,
                Keyword = page.Keyword,
                IsActive = page.IsActive
            };
        }

        public Task<List<Select2ItemDto>> GetSelectListAsync()
        {
            return _dbContext.Pages
                .Where(c => !c.Deleted)
                .OrderByDescending(c => c.Id)
                .Select(c => new Select2ItemDto
                {
                    Id = c.Id.ToString(),
                    Text = c.Title
                })
                .ToListAsync();
        }

        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var page = await _dbContext.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (page == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                page.Deleted = true;
                page.DeletedOn = DateTime.Now;
                _dbContext.Update(page);

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
            var propertyInfo = typeof(PageGridDto).GetProperty(nameof(PageGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var page = await _dbContext.Pages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (page == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                page.IsActive = !page.IsActive;
                page.UpdatedOn = DateTime.Now;

                _dbContext.Update(page);
                await _dbContext.SaveChangesAsync();

                if (page.IsActive)
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

        public async Task CreateAsync(PageCreateDto dto)
        {
            var page = new Page
            {
                Title = dto.Title,
                Keyword = dto.Keyword,
                Summary = dto.Summary,
                Body = dto.Body,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.Now,
                Language = dto.Language
            };
            _dbContext.Pages.Add(page);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandResultDto> UpdateAsync(PageUpdateDto dto)
        {
            var page = await _dbContext.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (page == null)
            {
                return CommandResultDto.NotFound();
            }

            page.Title = dto.Title;
            page.Summary = dto.Summary;
            page.Body = dto.Body;
            page.IsActive = dto.IsActive;
            page.UpdatedOn = DateTime.Now;

            _dbContext.Update(page);
            await _dbContext.SaveChangesAsync();

            return CommandResultDto.Successful();
        }
    }
}
