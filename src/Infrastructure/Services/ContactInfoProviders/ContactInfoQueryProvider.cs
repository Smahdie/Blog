using Core.Dtos.ContactInfoDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContactInfoProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContactInfoProviders
{
    public class ContactInfoQueryProvider : IContactInfoQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheSettings _memoryCacheSettings;

        public ContactInfoQueryProvider(
            ApplicationDbContext dbContext,
            IMemoryCache cache,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _dbContext = dbContext;
            _cache = cache;
            _memoryCacheSettings = memoryCacheSettings.Value;
        }

        public async Task<List<ContactInfoListDto>> GetAllAsync()
        {
            var serializedData = await _cache.GetOrCreateAsync(CacheKeys.ContactInfo(), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _memoryCacheSettings.ContactInfoExpiration;
                return GetAllFromDb();
            });
            var model = JsonSerializer.Deserialize<List<ContactInfoListDto>>(serializedData);
            return model;
        }

        private async Task<string> GetAllFromDb()
        {
            var data = await _dbContext.ContactInfos
                .Where(c => !c.Deleted && c.IsActive)
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
    }
}
