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

        public IActionResult Index(string aplicacao = null)
        {
            var projetos = Models.Projetos.InstanciarProjetos();

            if(!string.IsNullOrEmpty(aplicacao))
                projetos = projetos
                    .Where(x => x.Nome.Contains(aplicacao, System.StringComparison.OrdinalIgnoreCase) 
                        || x.DescricaoBreve.Contains(aplicacao) 
                        || x.PalavrasChaves.Contains(aplicacao, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

            @ViewData["projetos"] = projetos.OrderBy(x => x.DataLancamento).ToList();            

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
