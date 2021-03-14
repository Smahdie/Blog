using Core.Dtos.ContentDtos;
using Core.Dtos.CommonDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ContentProviders
{
    public interface IContentManager
    {
        Task<(List<ContentGridDto> Items, int TotalCount)> GetAllAsync(ContentGridDto dto);
        Task<ContentUpdateDto> GetAsync(int id);
        Task<DeleteResultDto> DeleteAsync(int id);
        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
        Task CreateAsync(ContentCreateDto dto);
        Task<CommandResultDto> UpdateAsync(ContentUpdateDto dto);
    }
}
