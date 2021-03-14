using Core.Dtos.TagDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.TagProviders
{
    public interface ITagQueryProvider
    {
        Task<List<TagListDto>> GetTopTagsAsync();
        Task<TagListDto> GetByIdAsync(int id);
    }
}
