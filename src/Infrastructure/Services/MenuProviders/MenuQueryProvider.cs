using Core.Dtos.CategoryDtos;
using Core.Dtos.MenuDtos;
using Core.Dtos.PageDtos;
using Core.Dtos.Settings;
using Core.Interfaces.MenuProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Component.SlugGenerator;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.MenuProviders
{
    public class MenuQueryProvider : IMenuQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheSettings _memoryCacheSettings;

        public MenuQueryProvider(
            ApplicationDbContext dbContext,
            IMemoryCache cache,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _dbContext = dbContext;
            _cache = cache;
            _memoryCacheSettings = memoryCacheSettings.Value;
        }

        public async Task<MenuDetailsDto> GetByKeywordAsync(string keyword, bool nested = false)
        {
            var serializedData = await _cache.GetOrCreateAsync(CacheKeys.Menu(keyword,nested), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _memoryCacheSettings.MenuExpiration;
                return GetByKeywordFromDb(keyword,nested);
            });

            var model = JsonSerializer.Deserialize<MenuDetailsDto>(serializedData);
            return model;
        }

        private async Task<string> GetByKeywordFromDb(string keyword, bool nested = false)
        {
            var query = _dbContext.Menus
                .Where(menu => menu.Keyword == keyword)
                .Include(menu => menu.Members).ThenInclude(member => member.Page)
                .Include(menu => menu.Members).ThenInclude(member => member.Category).ThenInclude(cat => cat.Translations);

            if (nested)
            {
                query = query.Include(menu => menu.Members).ThenInclude(member => member.Category).ThenInclude(cat => cat.Children).ThenInclude(child => child.Translations);
            }


            var data = await query

                .Select(menu => new MenuDetailsDto
                {
                    Title = menu.Title,
                    MenuMembers = menu.Members.OrderBy(member => member.Rank).Select(member => new MenuMemberDto
                    {
                        TargetType = member.TargetType,
                        Page = (!member.PageId.HasValue || member.Page.Deleted || !member.Page.IsActive ? null : new PageListItemDto
                        {
                            Id = member.Page.Id,
                            Title = member.Page.Title,
                            Keyword = member.Page.Keyword
                        }),
                        Category = (!member.CategoryId.HasValue || member.Category.Deleted || !member.Category.IsActive ? null : new CategoryListItemDto
                        {
                            Id = member.Category.Id,
                            Name = member.Category.Translations.First(t => t.Language == "fa").Name,
                            Slug = member.Category.Translations.First(t => t.Language == "fa").Name.GetSlug(true),
                            Children = member.Category.Children.Where(child => !child.Deleted && child.IsActive).Select(child => new CategoryListItemDto
                            {
                                Id = child.Id,
                                Name = child.Translations.First(t => t.Language == "fa").Name,
                                Slug = child.Translations.First(t => t.Language == "fa").Name.GetSlug(true)
                            }).ToList()
                        })

                    }).ToList()
                })
                .FirstOrDefaultAsync();

            var serializedData = JsonSerializer.Serialize(data);
            return serializedData;
        }
    }
}