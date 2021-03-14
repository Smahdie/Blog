using Core.Dtos.EmailDtos;
using Core.Interfaces.EmailProviders;
using System.Threading.Tasks;

namespace Infrastructure.Services.EmailProviders
{
    public class EmptyEmailManager : IEmailManager
    {
        public Task SendEmailAsync(SendEmailDto dto)
        {
            return Task.CompletedTask;
        }
    }
}
