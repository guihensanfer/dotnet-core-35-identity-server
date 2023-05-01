using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bom_Dev.Models;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Bom_Dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
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
