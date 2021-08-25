using Core.Dtos.PageDtos;
using Core.Dtos.Settings;
using Core.Interfaces.PageProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.PageProviders
{
    public class PageQuery : IPageQuery
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheSettings _memoryCacheSettings;

        public PageQuery(
            ApplicationDbContext dbContext,
            IMemoryCache cache,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _dbContext = dbContext;
            _cache = cache;
            _memoryCacheSettings = memoryCacheSettings.Value;
        }

        public async Task<PageDetailsDto> GetDetailsAsync(string keyword, string language)
        {
            var serializedData = await _cache.GetOrCreateAsync(CacheKeys.Page(keyword, language), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _memoryCacheSettings.PageExpiration;
                return GetDetailsFromDb(keyword, language);
            });

            var model = JsonSerializer.Deserialize<PageDetailsDto>(serializedData);
            return model;
        }

        private async Task<string> GetDetailsFromDb(string keyword, string language)
        {
            var data = await Query()
                .Where(p => p.Keyword == keyword)
                .Where(p => p.Language == language)
                .Select(p => new PageDetailsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Keyword = p.Keyword,
                    Summary = p.Summary,
                    Body = p.Body
                })
                .FirstOrDefaultAsync();

            var serializedData = JsonSerializer.Serialize(data);
            return serializedData;
        }

        private IQueryable<Page> Query()
        {
            return _dbContext.Pages
                .Where(p => !p.Deleted && p.IsActive);
        }
    }
}
