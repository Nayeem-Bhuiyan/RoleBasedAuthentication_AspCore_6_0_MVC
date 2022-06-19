using NayeemApplication.Services.MailService.Interface;
namespace NayeemApplication.Services.MailService
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private EmailService _emailService;

        public AuthMessageSender(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            await _emailService.Send(email, subject, message);
            //return Task.FromResult(0);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            await _emailService.Send(number, "SMS Auth", message);
            //return Task.FromResult(0);
        }
    }
}
