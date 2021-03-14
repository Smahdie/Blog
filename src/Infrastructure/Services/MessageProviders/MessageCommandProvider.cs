using Core.Dtos.CommonDtos;
using Core.Dtos.EmailDtos;
using Core.Dtos.MessageDtos;
using Core.Interfaces.EmailProviders;
using Core.Interfaces.MessageProviders;
using Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.MessageProviders
{
    public class MessageCommandProvider : IMessageCommandProvider
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailManager _emailManager;
        private readonly MailManagerSettings _mailManagerSettings;
        private readonly ILogger<MessageCommandProvider> _logger;

        public MessageCommandProvider(
            ApplicationDbContext dbContext,
            IEmailManager emailManager,
            IOptions<MailManagerSettings> mailManagerSettings,
            ILogger<MessageCommandProvider> logger)
        {
            _dbContext = dbContext;
            _emailManager = emailManager;
            _mailManagerSettings = mailManagerSettings.Value;
            _logger = logger;
        }

        public async Task<CommandResultDto> SendAsync(MessageSendDto dto)
        {
            try
            {
                var message = new Message
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Text = dto.Text,
                    IsRead = false,
                    CreatedOn = DateTime.Now
                };
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();

                var emailData = new SendEmailDto
                {
                    Subject = "پیام جدید در سایت",
                    Message = $"<b>نام:</b> {message.FirstName} {message.LastName}<br/><b>تلفن:</b> {message.PhoneNumber}<br/><b>ایمیل:</b> {message.Email}<br/><b>پیام:</b> {message.Text.Replace("\n", "<br/>")}",
                    FromEmail = _mailManagerSettings.AddressList.Support,
                    ToEmail = _mailManagerSettings.AddressList.Support.Email
                };
                _emailManager.SendEmailAsync(emailData);

                var result = CommandResultDto.Successful();
                result.Message = "پیام شما با موفقیت ارسال شد.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occured while sending message. FirstName: {FirstName} - LastName: {LastName} - PhoneNumber: {PhoneNumber} - Email: {Email} - Text: {Text}", dto.FirstName, dto.LastName, dto.PhoneNumber, dto.Email, dto.Text);
                return CommandResultDto.UnknownError();
            }
        }
    }
}
