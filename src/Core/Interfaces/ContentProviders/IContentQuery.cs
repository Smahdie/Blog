using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ContentProviders
{
    public interface IContentQuery
    {
        Task<ListHasMoreDto<ContentListDto>> GetTopAsync(TopContentRequestDto dto);

        Task<(List<ContentListDto> Items, int TotalCount)> GetAllAsync(ContentListRequestDto dto);

        Task<(List<ContentListDto> Items, int TotalCount)> SearchAsync(ContentSearchRequestDto dto);

        Task<ContentDetailsDto> GetDetailAsync(int id, ContentType? type = null);
    }
}
