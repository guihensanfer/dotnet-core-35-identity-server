using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bom_Dev.Controllers
{
    public class ViewComponentController : Controller
    {
        private readonly Data.Interface.IRepository _context;

        public ViewComponentController(Data.Interface.IRepository context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetCategoriesByParentIdFromPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var vs = path.Split('/');
                var len = vs.Length;
                // Para obter o nível acima                
                Category.OrderView order = (Category.OrderView)len + 1;                

                var result = await _context.GetCategories(new Optimization(Optimization.LoadedColumnsLevel.C), 
                    true, 
                    new System.Collections.Generic.List<Category.OrderView>() { order }, // Somente nivel +1 a frente
                    null, 
                    path);

                return Json(result);
            }
            
            return Json(null);
        }
    }
}
