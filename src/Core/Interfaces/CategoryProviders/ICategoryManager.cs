using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CategoryProviders
{
    public interface ICategoryManager
    {
        Task<(List<CategoryGridDto> Items, int TotalCount)> GetAllAsync(CategoryGridDto dto);
        Task<List<Select2ItemDto>> GetSelectListAsync(int? toExclude = null);
        Task<List<JsTreeNode>> GetTreeAsync();
        Task<CategoryUpdateDto> GetAsync(int id);
        Task<DeleteResultDto> DeleteAsync(int id);
        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
        Task CreateAsync(CategoryCreateDto dto);
        Task<CommandResultDto> UpdateAsync(CategoryUpdateDto dto);
    }
}
