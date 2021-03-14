using Core.Dtos.PageDtos;
using Core.Dtos.CommonDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.PageProviders
{
    public interface IPageManager
    {
        Task<(List<PageGridDto> Items, int TotalCount)> GetAllAsync(PageGridDto dto);
        Task<PageUpdateDto> GetAsync(int id);
        Task<List<Select2ItemDto>> GetSelectListAsync();
        Task<DeleteResultDto> DeleteAsync(int id);
        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
        Task CreateAsync(PageCreateDto dto);
        Task<CommandResultDto> UpdateAsync(PageUpdateDto dto);
    }
}
