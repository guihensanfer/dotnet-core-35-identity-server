using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bom_Dev.Models;
using System.Linq;

namespace Bom_Dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }        

        public IActionResult Index(string searchApp = null)
        {
            var projetos = API.Models.Projects.GetProjects();

            if(!string.IsNullOrWhiteSpace(searchApp))
                projetos = projetos
                    .Where(x => x.name.Contains(searchApp, System.StringComparison.OrdinalIgnoreCase) 
                        || x.description.Contains(searchApp) 
                        || x.keywords.Contains(searchApp, System.StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.releaseDate)
                    .ToList();

            @ViewData["projetos"] = projetos;            

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
