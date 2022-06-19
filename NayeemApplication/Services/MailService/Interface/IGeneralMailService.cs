namespace NayeemApplication.Services.MailService.Interface
{
   public interface IGeneralMailService
    {
        Task<bool> SendEmailAsync(string toAddr, string subject, string body);
    }
}
