using Core.Dtos.Settings;
using Core.Dtos.TagDtos;
using Core.Interfaces.TagProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Component.SlugGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.TagProviders
{
    public class TagQueryProvider : ITagQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly WebAppSettings _appSettings;

        public TagQueryProvider(
            ApplicationDbContext dbContext,
            IOptions<WebAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public Task<List<TagListDto>> GetTopTagsAsync()
        {
            return _dbContext.Tags
                .OrderBy(a=> Guid.NewGuid())
                .Take(_appSettings.TopTagsCount)
                .Select(a => new TagListDto 
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Name.GetSlug(true)
                })
                .ToListAsync();
        }

        public Task<TagListDto> GetByIdAsync(int id)
        {
            return _dbContext.Tags
                .Where(t => t.Id == id)
                .Select(a => new TagListDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Name.GetSlug(true)
                })
                .FirstOrDefaultAsync();
        }
    }
}
