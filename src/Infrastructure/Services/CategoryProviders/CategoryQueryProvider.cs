using Core.Dtos.CategoryDtos;
using Core.Interfaces.CategoryProviders;
using Microsoft.EntityFrameworkCore;
using Component.SlugGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.CategoryProviders
{
    public class CategoryQueryProvider : ICategoryQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryQueryProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<CategoryListItemDto>> GetListAsync(int? ParentId)
        {
            return _dbContext.Categories
                .Where(c => !c.Deleted && c.IsActive && c.ParentId == ParentId)
                .Select(c => new CategoryListItemDto 
                { 
                    Id = c.Id,
                    Name = c.Translations.First(t=> t.Language == "fa").Name,
                    Slug = c.Translations.First(t => t.Language == "fa").Name.GetSlug(true)
                })
                .ToListAsync();
        }

        public Task<CategoryListItemDto> GetByIdAsync(int id)
        {
            return _dbContext.Categories
               .Where(c => !c.Deleted && c.IsActive && c.Id == id)
               .Select(c => new CategoryListItemDto
               {
                   Id = c.Id,
                   Name = c.Translations.First(t => t.Language == "fa").Name,
                   Slug = c.Translations.First(t => t.Language == "fa").Name.GetSlug(true)
               })
               .FirstOrDefaultAsync();
        }
    }
}
