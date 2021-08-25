using Core.Dtos.ContactInfoDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContactInfoProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContactInfoProviders
{
    public class ContactInfoQuery : IContactInfoQuery
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheSettings _memoryCacheSettings;

        public ContactInfoQuery(
            ApplicationDbContext dbContext,
            IMemoryCache cache,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _dbContext = dbContext;
            _cache = cache;
            _memoryCacheSettings = memoryCacheSettings.Value;
        }

        public async Task<List<ContactInfoListDto>> GetAllAsync(string language)
        {
            var serializedData = await _cache.GetOrCreateAsync(CacheKeys.ContactInfo(language), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _memoryCacheSettings.ContactInfoExpiration;
                return GetAllFromDb(language);
            });

            var model = JsonSerializer.Deserialize<List<ContactInfoListDto>>(serializedData);
            return model;
        }

        private async Task<string> GetAllFromDb(string language)
        {
            var data = await Query()
                .Where(c => c.Language == language)
                .Select(c => new ContactInfoListDto
                {
                    Id = c.Id,
                    ContactType = c.ContactType,
                    Value = c.Value
                })
                .ToListAsync();

            var serializedData = JsonSerializer.Serialize(data);
            return serializedData;
        }

        private IQueryable<ContactInfo> Query()
        {
            return _dbContext.ContactInfos
                .Where(c => !c.Deleted && c.IsActive);
        }
    }
}
