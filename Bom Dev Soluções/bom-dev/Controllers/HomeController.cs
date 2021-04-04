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
            var projetos = Models.Projetos.InstanciarProjetos();            

            if(!string.IsNullOrWhiteSpace(searchApp))
                projetos = projetos
                    .Where(x => x.Nome.Contains(searchApp, System.StringComparison.OrdinalIgnoreCase) 
                        || x.DescricaoBreve.Contains(searchApp) 
                        || x.PalavrasChaves.Contains(searchApp, System.StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.DataLancamento)
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
