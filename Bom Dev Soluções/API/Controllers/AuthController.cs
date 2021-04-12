using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    [AllowAnonymous]
    public class ProjectsController : ControllerBase
    {                
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(Models.Projects.GetProjects());
        }
    }
}
