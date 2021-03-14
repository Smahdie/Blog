using Core.Dtos.CommonDtos;
using Core.Dtos.MessageDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.MessageProviders
{
    public interface IMessageCommandProvider
    {
        Task<CommandResultDto> SendAsync(MessageSendDto dto);
    }
}
