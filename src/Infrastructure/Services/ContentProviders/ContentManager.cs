using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.ContentProviders;
using Core.Interfaces.TagProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContentProviders
{
    public class ContentManager: IContentManager
    {
        private readonly ILogger<ContentManager> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITagManager _tagManager;
        private readonly PanelAppSettings _appSettings;        

        public ContentManager(
            ILogger<ContentManager> logger,
            ApplicationDbContext dbContext,
            ITagManager tagManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _logger = logger;
            _dbContext = dbContext;
            _tagManager = tagManager;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<ContentGridDto> Items, int TotalCount)> GetAllAsync(ContentGridDto dto)
        {
            var query = _dbContext.Contents.Where(c => !c.Deleted);
            
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            if (dto.TagId.HasValue)
            {
                query = query.Include(c => c.Tags).Where(c => c.Tags.Any(t=> t.TagId == dto.TagId));
            }

            if (dto.CategoryId.HasValue)
            {
                query = query.Include(c => c.Categories).Where(c => c.Categories.Any(t => t.CategoryId == dto.CategoryId));
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Title), c => c.Title.Contains(dto.Title.Trim()));
            query = query.WhereIf(dto.IsActive.HasValue, c => c.IsActive == dto.IsActive);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(dto.UpdatedOn))
            {
                var updatedOnDateTime = DateTime.Parse(dto.UpdatedOn);
                query = query.Where(c => c.UpdatedOn!=null && c.UpdatedOn > updatedOnDateTime.Date && c.UpdatedOn < updatedOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(c => new ContentGridDto
            {
                Id = c.Id,
                IsActive = c.IsActive,
                Title = c.Title,
                Language = c.Language,
                ViewCount = c.ViewCount,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(c.CreatedOn),
                UpdatedOn = (c.UpdatedOn!=null? PersianDateHelper.ConvertToLocalDateTime(c.UpdatedOn.Value) : "---")
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<ContentUpdateDto> GetAsync(int id)
        {
            var content = await _dbContext.Contents
                .Where(c => c.Id == id && !c.Deleted)
                .Include(c => c.Categories)
                .Include(c => c.Tags)
                .ThenInclude(t=>t.Tag)
                .FirstOrDefaultAsync();
            if (content == null)
            {
                return null;
            }

            return new ContentUpdateDto
            {
                Id = content.Id,
                Language = content.Language,
                Title = content.Title,
                Summary = content.Summary,
                Body = content.Body,
                ImagePath = content.ImagePath,
                IsActive = content.IsActive,
                SelectedCategories = content.Categories.Select(c=>c.CategoryId).ToList(),
                SelectedTags = content.Tags.Select(t=> new Select2ItemDto { Id = t.TagId.ToString(), Text = t.Tag.Name }).ToList()
            };
        }

        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var content = await _dbContext.Contents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (content == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            try
            {
                content.Deleted = true;
                content.DeletedOn = DateTime.Now;

                _dbContext.Update(content);
                await _dbContext.SaveChangesAsync();
                return DeleteResultDto.Successful(id.ToString());
            }
            catch
            {
                return DeleteResultDto.UnknownError(id.ToString());
            }
        }
        
        public async Task<ChangeStatusResultDto> ChangeStatusAsync(int id)
        {
            var propertyInfo = typeof(ContentGridDto).GetProperty(nameof(ContentGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var content = await _dbContext.Contents
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (content == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                content.IsActive = !content.IsActive;
                content.UpdatedOn = DateTime.Now;

                _dbContext.Update(content);
                await _dbContext.SaveChangesAsync();

                if (content.IsActive)
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
        
        public async Task CreateAsync(ContentCreateDto dto)
        {
            var contentCats = GetContentCategories(dto.Categories);

            var content = new Content
            {
                Title = dto.Title,
                Summary = dto.Summary,
                Body = dto.Body,
                ImagePath = dto.ImagePath,
                IsActive = dto.IsActive,
                Language = dto.Language,
                CreatedOn = DateTime.Now
            };

            var (dbTags, newTags) = SeperateTags(dto.Tags);

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var createdTags = await _tagManager.CreateAsync(newTags, dto.Language);
                dbTags.AddRange(createdTags);

                _dbContext.Contents.Add(content);
                await _dbContext.SaveChangesAsync();

                _dbContext.Content2Tags.AddRange(dbTags.Select(tagId => new Content2Tag {
                    TagId = tagId,
                    ContentId = content.Id
                }));
                _dbContext.Content2Categories.AddRange(contentCats.Select(catId => new Content2Category { 
                    CategoryId = catId,
                    ContentId = content.Id
                }));
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occured while creating a new content");
            }
        }

        public async Task<CommandResultDto> UpdateAsync(ContentUpdateDto dto)
        {
            var content = await _dbContext.Contents.FirstOrDefaultAsync(c => !c.Deleted && c.Id == dto.Id);
            if(content == null)
            {
                return CommandResultDto.NotFound();
            }

            var contentCats = GetContentCategories(dto.Categories);

            var (dbTags, newTags) = SeperateTags(dto.Tags);

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var createdTags = await _tagManager.CreateAsync(newTags, content.Language);
                dbTags.AddRange(createdTags);

                content.Title = dto.Title;
                content.Summary = dto.Summary;
                content.Body = dto.Body;
                content.ImagePath = dto.ImagePath;
                content.IsActive = dto.IsActive;
                content.UpdatedOn = DateTime.Now;
                _dbContext.Update(content);

                var currentTags = _dbContext.Content2Tags.Where(tag => tag.ContentId == dto.Id).Select(tag => tag.TagId);
                var tagsToInsert = dbTags.Where(tag => !currentTags.Contains(tag));
                _dbContext.Content2Tags.AddRange(tagsToInsert.Select(tagId => new Content2Tag
                {
                    TagId = tagId,
                    ContentId = content.Id
                }));
                _dbContext.Content2Tags.RemoveRange(_dbContext.Content2Tags.Where(t => t.ContentId == dto.Id && !dbTags.Contains(t.TagId)));


                var currentCats = _dbContext.Content2Categories.Where(c => c.ContentId == dto.Id).Select(c => c.CategoryId);
                var catsToInsert = contentCats.Where(cat => !currentCats.Contains(cat));
                _dbContext.Content2Categories.AddRange(catsToInsert.Select(catId => new Content2Category
                {
                    CategoryId = catId,
                    ContentId = content.Id
                }));
                _dbContext.Content2Categories.RemoveRange(_dbContext.Content2Categories.Where(c => c.ContentId == dto.Id && !contentCats.Contains(c.CategoryId)));
                await _dbContext.SaveChangesAsync();

                transaction.Commit();

                return CommandResultDto.Successful();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating content");
                return CommandResultDto.UnknownError();
            }
        }

        private List<int> GetContentCategories(string cats)
        {
            var split = !string.IsNullOrWhiteSpace(cats) ? cats.Split(',') : new string[] { };
            return split.Select(s => int.Parse(s)).ToList();
        }

        private (List<int> dbTags, List<string> newTags) SeperateTags(string[] tags)
        {
            var dbTags = new List<int>();
            var newTags = new List<string>();
            if(tags == null)
            {
                return (dbTags, newTags);
            }
            foreach (var tag in tags)
            {
                if (int.TryParse(tag, out int tagId))
                {
                    dbTags.Add(tagId);
                }
                else
                {
                    newTags.Add(tag);
                }
            }
            return (dbTags, newTags);
        }
    }
}
