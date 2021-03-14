using Core.Dtos.CommonDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.CaptchaProviders
{
    public interface ICaptchaManager
    {
        Task<CommandResultDto> IsValidAsync(string token);
    }
}
