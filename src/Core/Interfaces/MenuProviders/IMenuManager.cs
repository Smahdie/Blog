using Core.Dtos.CommonDtos;
using Core.Dtos.MenuDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.MenuProviders
{
    public interface IMenuManager
    {
        Task<(List<MenuGridDto> Items, int TotalCount)> GetAllAsync(MenuGridDto dto);
        Task<MenuUpdateDto> GetAsync(int id);
        Task<CommandResultDto> CreateAsync(MenuCreateDto dto);
        Task<CommandResultDto> UpdateAsync(MenuUpdateDto dto);
    }
}
