using Core.Dtos.CategoryDtos;
using Core.Interfaces.CategoryProviders;
using Microsoft.EntityFrameworkCore;
using Component.SlugGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.CategoryProviders
{
    public class CategoryQuery : ICategoryQuery
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<CategoryListItemDto>> GetListAsync(int? ParentId, string language)
        {
            return _dbContext.Categories
                .Where(c => !c.Deleted && c.IsActive)
                .Where(c=> c.ParentId == ParentId)
                .Select(c => new CategoryListItemDto 
                { 
                    Id = c.Id,
                    Name = c.Translations.First(t=> t.Language == language).Name,
                    Slug = c.Translations.First(t => t.Language == language).Name.GetSlug(true)
                })
                .ToListAsync();
        }

        public Task<CategoryListItemDto> GetByIdAsync(int id, string language)
        {
            return _dbContext.Categories
               .Where(c => !c.Deleted && c.IsActive && c.Id == id)
               .Select(c => new CategoryListItemDto
               {
                   Id = c.Id,
                   Name = c.Translations.First(t => t.Language == language).Name,
                   Slug = c.Translations.First(t => t.Language == language).Name.GetSlug(true)
               })
               .FirstOrDefaultAsync();
        }
    }
}
