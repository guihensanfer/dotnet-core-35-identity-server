using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Data.Identity;

namespace Bom_Dev.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<PersonalUser> _userManager;
        private readonly SignInManager<PersonalUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<PersonalUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<PersonalUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name="E-mail")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name="Senha")]
            public string Password { get; set; }

            [Display(Name = "Lembrar-me?")]
            public bool RememberMe { get; set; }
        }        

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            //returnUrl = returnUrl ?? Url.Content("~/");

            //// Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //ReturnUrl = returnUrl;

            // Código retirado do ExternalLoginModel
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(Startup.IdentityServerScheme, redirectUrl);
            
            return new ChallengeResult(Startup.IdentityServerScheme, properties);
            //return RedirectToPage("./ExternalLogin", new { });
        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);                                                                                                                            

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");                    

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
