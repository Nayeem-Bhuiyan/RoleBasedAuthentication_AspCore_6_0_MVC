using NayeemApplication.Models;
namespace NayeemApplication.Services.MailService.Interface
{
   public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendFileEmailAsync(string ToEmail, string Subject, string Body, List<IFormFile> Attachments);
        Task<bool> SendTextEmailAsync(string ToEmail, string Subject, string Body);
    }
}
