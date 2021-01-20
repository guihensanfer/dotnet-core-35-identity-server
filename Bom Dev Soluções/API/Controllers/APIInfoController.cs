using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class APIInfoController : Controller
    {
        public IActionResult Index()
        {
            return new JsonResult(new
            {
                Version = new Version()
            });
        }

        public IActionResult Get()
        {
             return new JsonResult(new
            {
                Version = new Version()
            });
        }
    }
}
