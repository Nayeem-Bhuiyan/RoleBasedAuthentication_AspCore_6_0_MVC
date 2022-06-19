namespace NayeemApplication.Services.MailService.Interface
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
