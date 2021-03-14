using Core.Dtos.CategoryDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CategoryProviders
{
    public interface ICategoryQueryProvider
    {
        Task<List<CategoryListItemDto>> GetListAsync(int? ParentId);

        Task<CategoryListItemDto> GetByIdAsync(int id);
    }
}
