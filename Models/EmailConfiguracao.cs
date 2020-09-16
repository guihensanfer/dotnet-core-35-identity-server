using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using System.IO;

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
            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recursos", "Assinatura.html");

            try
            {
                return File.ReadAllText(caminho);
            }
            catch{}     

            return null;
        }
    }
}