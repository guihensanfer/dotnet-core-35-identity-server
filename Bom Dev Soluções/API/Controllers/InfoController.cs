using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : Controller
    {        
        public IActionResult Get() => new JsonResult(new
        {
            Version = new Version()
        });

        [HttpGet("GetProjects")]
        public IActionResult GetProjects() => new JsonResult(Projects.InstanciarProjetos());
    }
}
