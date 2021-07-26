using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Dtos.TagDtos;
using Core.Helpers;
using Core.Interfaces.ContentProviders;
using Core.Models;
using Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Component.SlugGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContentProviders
{
    public class ContentQueryProvider : IContentQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly WebAppSettings _appSettings;

        public ContentQueryProvider(
            ApplicationDbContext dbContext,
            IOptions<WebAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<ListHasMoreDto<ContentListDto>> GetTopAsync(TopContentRequestDto dto)
        {
            var query = Query();
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Type.HasValue, c => c.Type == dto.Type);

            var take = _appSettings.TopArticlesCount;
            var takePlusOne = take + 1;

            query = query.Take(takePlusOne);
            var items = await query.Select(content => new ContentListDto
            {
                Id = content.Id,
                Title = content.Title,
                Slug = content.Title.GetSlug(true),
                Summary = content.Summary,
                ImagePath = content.ImagePath,
                PersianCreatedOn = PersianDateHelper.ConvertToLocalDateTime(content.CreatedOn),
                CreatedOn = content.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            }).ToListAsync();

            var hasMore = items.Count > take;

            return new ListHasMoreDto<ContentListDto>
            {
                Items = items.Take(take).ToList(),
                HasMore = hasMore
            };
        }

        public async Task<(List<ContentListDto> Items, int TotalCount)> GetAllAsync(ContentListRequestDto dto)
        {
            var query = Query();
            query = query.WhereIf(dto.TagId.HasValue, c => c.Tags.Any(t => t.TagId == dto.TagId));
            query = query.WhereIf(dto.CategoryId.HasValue, c => c.Categories.Any(t => t.CategoryId == dto.CategoryId || t.Category.ParentId == dto.CategoryId));
            query = query.WhereIf(dto.Type.HasValue, c => c.Type == dto.Type);
            
            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.PageIndex - 1) * take;

            var items = await query
                .OrderByDescending(c=>c.Id)
                .Skip(skip)
                .Take(take)
                .Select(content => new ContentListDto 
                {
                    Id = content.Id,
                    Title = content.Title,
                    Slug = content.Title.GetSlug(true),
                    Summary = content.Summary,
                    ImagePath = content.ImagePath,
                    PersianCreatedOn = PersianDateHelper.ConvertToLocalDateTime(content.CreatedOn),
                    CreatedOn = content.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<ContentListDto> Items, int TotalCount)> SearchAsync(ContentSearchRequestDto dto)
        {
            dto.Keyword = dto.Keyword?.Trim();
            var query = Query();
            query = query.WhereIf(dto.Type.HasValue, c => c.Type == dto.Type);
            query = query.Where(c => c.Title.Contains(dto.Keyword) || c.Summary.Contains(dto.Keyword));

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.PageIndex - 1) * take;

            var items = await query
                .OrderByDescending(c => c.Id)
                .Skip(skip)
                .Take(take)
                .Select(content => new ContentListDto
                {
                    Id = content.Id,
                    Title = content.Title,
                    Slug = content.Title.GetSlug(true),
                    Summary = content.Summary,
                    ImagePath = content.ImagePath,
                    PersianCreatedOn = PersianDateHelper.ConvertToLocalDateTime(content.CreatedOn),
                    CreatedOn = content.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<ContentDetailsDto> GetDetailAsync(int id, ContentType? type = null)
        {
            return Query()
                .Where(c => c.Id == id)
                .WhereIf(type.HasValue, c => c.Type == type)
                .Select(c => new ContentDetailsDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Slug = c.Title.GetSlug(true),
                    Summary = c.Summary,
                    Type = c.Type,
                    Body = c.Body,
                    ImagePath = c.ImagePath,
                    ViewCount = c.ViewCount,
                    PersianCreatedOn = PersianDateHelper.ConvertToLocalDateTime(c.CreatedOn),
                    CreatedOn = c.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                    Tags = c.Tags
                            .Select(t => new TagListDto
                            {
                                Id = t.TagId,
                                Name = t.Tag.Name,
                                Slug = t.Tag.Name.GetSlug(true)
                            })
                            .ToList(),
                    Categories = c.Categories
                                  .Select(cat => new CategoryListItemDto
                                  {
                                      Id = cat.CategoryId,
                                      Name = cat.Category.Translations.First(t => t.Language == "fa").Name,
                                      Slug = cat.Category.Translations.First(t => t.Language == "fa").Name.GetSlug(true)
                                  })
                                  .ToList()
                })
                .FirstOrDefaultAsync();
        }

        private IQueryable<Content> Query() 
        {
            return _dbContext.Contents.Where(c => !c.Deleted && c.IsActive);
        }
    }
}
