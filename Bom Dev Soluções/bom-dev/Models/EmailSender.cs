using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Bom_Dev.Models
{
    public class EmailSender : IEmailSender
    {        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            EmailConfig emailConfig = new EmailConfig();            
            var client = new SmtpClient(emailConfig.host)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(emailConfig.email, emailConfig.password),
                Port = emailConfig.port
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailConfig.email, "Bom Dev Software House"),
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
        public EmailConfig()
        {            
            host = ConfigurationManager.AppSettings["BomDevEmail:Host"];
            email = ConfigurationManager.AppSettings["BomDevEmail:Email"];
            password = ConfigurationManager.AppSettings["BomDevEmail:Password"];
            port = short.Parse(ConfigurationManager.AppSettings["BomDevEmail:Port"]);
        }

        public string host { get; private set; }
        public string email { get; private set; }
        public string password { get; private set; }
        public short port { get; private set; }

        public static string CorpoEmailConfirmarEmail(string linkConfirmacao)
        {
            return $"Olá, confirme seu e-mail <a href='{linkConfirmacao}'>clicando aqui</a>.{ObterAssinaturaPadraoHTML()}";
        }

        public static string CorpoEmailEsqueciSenha(string linkConfirmacao)
        {
            return $"Olá, resete sua senha <a href='{linkConfirmacao}'>clicando aqui</a>.{ObterAssinaturaPadraoHTML()}";
        }

        private static string ObterAssinaturaPadraoHTML()
        {
            return ConfigurationManager.AppSettings["BomDevEmail:SignatureHTML"];
        }
    }
}