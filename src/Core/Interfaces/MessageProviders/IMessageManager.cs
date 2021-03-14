using Core.Dtos.CommonDtos;
using Core.Dtos.MessageDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.MessageProviders
{
    public interface IMessageManager
    {
        Task<(List<MessageGridDto> Items, int TotalCount)> GetAllAsync(MessageGridDto dto);

        Task<MessageDetailsDto> GetAsync(int id);

        Task<DeleteResultDto> DeleteAsync(int id);

        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
    }
}
