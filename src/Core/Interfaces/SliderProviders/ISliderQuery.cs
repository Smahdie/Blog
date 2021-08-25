using Core.Dtos.SliderDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.SliderProviders
{
    public interface ISliderQuery
    {
        Task<List<SliderListDto>> GetListAsync(string language);
    }
}
