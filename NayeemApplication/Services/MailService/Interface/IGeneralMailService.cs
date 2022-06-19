namespace NayeemApplication.Services.MailService.Interface
{
   public interface IGeneralMailService
    {
        Task SendEmailAsync(string toAddr, string subject, string body);
    }
}
