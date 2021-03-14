using Core.Dtos.SliderDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.SliderProviders
{
    public interface ISliderQueryProvider
    {
        Task<List<SliderListDto>> GetListAsync();
    }
}
