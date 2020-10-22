using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bom_Dev.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Bom_Dev.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<BomDevUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<BomDevUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Email{get;set;}

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnGet(string email = null)
        {            
            if(!string.IsNullOrEmpty(email))
            {
                var user = await _userManager.FindByEmailAsync(email);

                if(user is null)                
                    return NotFound($"Usuário com e-mail {email} não encontrado.");

                Email = email;
            }            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail informado não cadastrado.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirme seu e-mail",
                Models.EmailConfiguracao.CorpoEmailConfirmarEmail(HtmlEncoder.Default.Encode(callbackUrl))); 

            ModelState.AddModelError(string.Empty, "E-mail enviado, por favor verifique seu e-mail.");
            return Page();
        }
    }
}
