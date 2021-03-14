using Core.Dtos.EmailDtos;
using Core.Interfaces.EmailProviders;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EmailProviders
{
    public class MailKitEmailManager : IEmailManager
    {
        private readonly MailKitServerSettings _serverSettings;
        public MailKitEmailManager(IOptions<MailKitServerSettings> serverSettings)
        {
            _serverSettings = serverSettings.Value;
        }

        public async Task SendEmailAsync(SendEmailDto dto)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(dto.FromEmail.Email));
            message.To.Add(MailboxAddress.Parse(dto.ToEmail));
            message.Subject = dto.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = dto.Message };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_serverSettings.SmtpServer, _serverSettings.SmtpPort, SecureSocketOptions.None);
            smtp.Authenticate(dto.FromEmail.Email, dto.FromEmail.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}
