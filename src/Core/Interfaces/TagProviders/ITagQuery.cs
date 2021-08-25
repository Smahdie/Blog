using Core.Dtos.TagDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.TagProviders
{
    public interface ITagQuery
    {
        Task<List<TagListDto>> GetTopTagsAsync(string language);

        Task<TagListDto> GetByIdAsync(int id);
    }
}
