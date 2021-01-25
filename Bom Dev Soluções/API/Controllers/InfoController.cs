using Microsoft.AspNetCore.Mvc;
using API.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class InfoController : Controller
    {
        [HttpGet]
        public IActionResult Get() => new JsonResult(new
        {
            versions = Version.GetVersions()
        });

        [HttpGet("GetProjects")]
        public IActionResult GetProjects() => new JsonResult(new 
        { 
            projects = Projects.GetProjects()         
        });
    }
}
