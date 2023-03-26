using Data.Extensions;
using Data.Interface;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Data.Models.Optimization;

namespace Bom_Dev.Areas.Adm.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [Area("Adm")]    
    public class CategoryController : Controller
    {
        private readonly IRepository _context;

        public CategoryController(IRepository context)
        {
            _context = context;
        }

        // GET: Adm/Category
        public async Task<IActionResult> Index(string enabled, string order)
        {
            bool? _enabled = enabled.ToNullableBool();
            int? _order = order.ToNullableInt();

            return View(await _context.GetCategories(new Optimization(LoadedColumnsLevel.B), 
                _enabled,
                _order.HasValue ?
                    new List<Category.OrderView>() { (Category.OrderView)_order.Value } :
                    null));
        }

        [HttpPost]
        public async Task<JsonResult> GetCategoriesByOrder(int order)
        {
            Category.OrderView orderView = (Category.OrderView)order;

            var result = await _context.GetCategories(new Optimization(LoadedColumnsLevel.C), true, new List<Category.OrderView>() {
                orderView
            });            

            return Json(result);
        }

        // GET: Adm/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.GetCategoryById(id.GetValueOrDefault());
                
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Adm/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adm/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Description,Url,Enabled,Order,ParentCategoryId,Path,Index")] Category category)
        {
            if (ModelState.IsValid)
            {
                bool validationByName = await _context.ExistsCategoryByName(category.Name);

                if (validationByName)
                {
                    ViewData.Add("msg", "Nome da categoria já existente.");

                    return View(category);
                }

                await _context.InsertCategory(category);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Adm/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.GetCategoryById(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Adm/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,DateCreated,Name,Description,Url,Enabled,Order,ParentCategoryId,Path,Index")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateCategory(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Adm/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            

            var category = await _context.GetCategoryById(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Adm/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if exists parents categories
            if (await _context.ExistsParentCategory(id))
            {
                ViewData.Add("msg", "Não é possível concluir, exclua primeiro as categorias filhas vinculadas.");

                return await Delete(id);
            }

            await _context.DeleteCategory(id);
            
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            var category = _context.GetCategoryById(id);

            return category != null;
        }
    }
}
