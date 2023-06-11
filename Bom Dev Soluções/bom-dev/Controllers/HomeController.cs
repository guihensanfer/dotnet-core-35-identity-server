using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bom_Dev.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using Data.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Data.Identity;

namespace Bom_Dev.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly Data.Interface.IRepository _repository;
        private readonly UserManager<PersonalUser> _userManager;

        public HomeController(ILogger<HomeController> logger, Data.Interface.IRepository repository, UserManager<PersonalUser> userManager, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _repository = repository;
            _userManager = userManager;
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

        public IActionResult About()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            string userId = null;
            var user = await _userManager.GetUserAsync(User);

            if(user != null)
            {
                userId = user.Id;
            }

            await _repository.InsertErrorLog(new ErrorLogs()
            {
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                Language = System.Globalization.CultureInfo.CurrentCulture.Name,
                Message = HttpContext.TraceIdentifier,
                RequestMethod = "GET",
                RequestUrl = HttpContext.Request.GetEncodedUrl(),
                StackTrace = null,
                Title = "150520231123-GenericErrorController",
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString(),
                UserId = userId
            });

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
