namespace NayeemApplication.Services.MailService.Interface
{
   public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
