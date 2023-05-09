using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bom_Dev.Models;
using System.Linq;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;

namespace Bom_Dev.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
        private string _currentLanguage;

        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                {
                    return _currentLanguage;
                }



                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.Name.ToLower();
                }

                return _currentLanguage;
            }
        }

        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            

            return RedirectToAction("Index", new { culture = lang });
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {
            var teste = _localizer["teste"];

            return View();
        }                      

        public IActionResult Sobre()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
