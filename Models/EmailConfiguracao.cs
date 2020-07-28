using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Bom_Dev.Models
{
    public class EmailConfiguracao : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp-mail.outlook.com") {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential("g.hs@live.com", "GH2,7,6,1"),
                Port = 25
            };
            var mailMessage = new MailMessage {
                From = new MailAddress("g.hs@live.com", "Bom Dev Software House"),      
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage     
            };
            mailMessage.To.Add(email);                        
            
            return client.SendMailAsync(mailMessage);
        }
    }
}