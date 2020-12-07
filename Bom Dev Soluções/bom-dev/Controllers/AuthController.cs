using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bom_Dev.Controllers
{    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public AuthController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }                  

        [HttpGet]       
        [Route("get")] 
        public string Get()
        {
            _logger.LogInformation("teste");
return "teste";
            // return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
