using Core.Dtos.CommonDtos;
using Core.Dtos.TagDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.TagProviders
{
    public interface ITagManager
    {
        Task<(List<TagGridDto> Items, int TotalCount)> GetAllAsync(TagGridDto dto);
        Task<Select2PagedResult> GetSelectListAsync(string term, int page);
        Task<List<int>> CreateAsync(IEnumerable<string> words);
        Task<DeleteResultDto> DeleteAsync(int id);
    }
}
