namespace Core.Dtos.EmailDtos
{
    public class SendEmailDto
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string ToEmail { get; set; }

        public SenderEmailDto FromEmail { get; set; }
    }
}
