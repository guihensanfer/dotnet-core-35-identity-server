using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuthController : ControllerBase
    {        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Acesso liberado!" };
        }
    }
}
