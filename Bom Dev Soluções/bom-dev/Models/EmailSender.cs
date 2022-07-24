using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bom_Dev.Models
{
    public class EmailSender : IEmailSender
    {        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {             
            var client = new SmtpClient(EmailConfig.host)
            {
                UseDefaultCredentials = false,
                EnableSsl = EmailConfig.enableSsl,
                Credentials = new NetworkCredential(EmailConfig.email, EmailConfig.password),
                Port = EmailConfig.port
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(EmailConfig.email, EmailConfig.displayName),
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }       
    }

    public class EmailConfig
    {               
        public static string host { get; internal set; }
        public static string email { get; internal set; }
        public static string password { get; internal set; }
        public static short port { get; internal set; }
        public static bool enableSsl { get; internal set; }
        public static string displayName { get; internal set; } = "Bom Terrários Floricultura";
        public static string signatureHTML { get; internal set; }        

        public static string CorpoEmailConfirmarEmail(string linkConfirmacao)
        {
            return $"Olá, confirme seu e-mail <a href='{linkConfirmacao}'>clicando aqui</a>.{signatureHTML}";
        }

        public static string CorpoEmailEsqueciSenha(string linkConfirmacao)
        {
            return $"Olá, resete sua senha <a href='{linkConfirmacao}'>clicando aqui</a>.{signatureHTML}";
        }
    }
}