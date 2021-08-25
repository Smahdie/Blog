using Core.Dtos.CategoryDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CategoryProviders
{
    public interface ICategoryQuery
    {
        Task<List<CategoryListItemDto>> GetListAsync(int? ParentId, string language);

        Task<CategoryListItemDto> GetByIdAsync(int id, string language);
    }
}
