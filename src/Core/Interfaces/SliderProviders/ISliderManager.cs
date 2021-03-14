using Core.Dtos.CommonDtos;
using Core.Dtos.SliderDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.SliderProviders
{
    public interface ISliderManager
    {
        Task<(List<SliderGridDto> Items, int TotalCount)> GetAllAsync(SliderGridDto dto);
        Task<SliderUpdateDto> GetAsync(int id);
        Task<DeleteResultDto> DeleteAsync(int id);
        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
        Task CreateAsync(SliderCreateDto dto);
        Task<CommandResultDto> UpdateAsync(SliderUpdateDto dto);
    }
}
