using Core.Dtos.EmailDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.EmailProviders
{
    public interface IEmailManager
    {
        Task SendEmailAsync(SendEmailDto dto);
    }
}
