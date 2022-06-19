using NayeemApplication.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
namespace NayeemApplication.Services.MailService
{
    public class EmailService
    {

        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task Send(string toAddr, string subject, string body)
        {
            var fromAddress = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            var toAddress = new MailAddress(toAddr);
            var smtp = new SmtpClient
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _mailSettings.Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}
