using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : Controller
    {        
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                Version = new Version()
            });
        }
    }
}
