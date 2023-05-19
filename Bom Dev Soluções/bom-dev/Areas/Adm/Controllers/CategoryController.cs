using Bom_Dev.Areas.Adm.Models;
using Data.Extensions;
using Data.Identity;
using Data.Interface;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Bom_Dev.Areas.Adm.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [Area("Adm")]    
    public class CategoryController : Controller
    {
        private readonly IRepository _context;
        private readonly UserManager<PersonalUser> _userManager;
        private readonly IViewLocalizer _localizer;

        public CategoryController(IRepository context, IViewLocalizer localizer, UserManager<PersonalUser> userManager)
        {
            _context = context;
            _localizer = localizer;
            _userManager = userManager;
        }

        // GET: Adm/Category
        public async Task<IActionResult> Index(bool? enabled, int? order, string name, int? parentCategoryId, bool? exportToExcel, int? page)
        {
            int itensPerPage = Optimization.SizePerPageDefault;
            bool _exportToExcel = exportToExcel.HasValue && exportToExcel.Value;

            if (!parentCategoryId.HasValue && !order.HasValue)
            {
                // parentCategoryId is used for navagation chieldrens categories                

                order = (int)Category.OrderView.First;
            }

            if(_exportToExcel)
            {
                // get all data 

                page = 1;
                itensPerPage = int.MaxValue;
            }           

            var data = await _context.GetCategories(
                 op: new Optimization(Optimization.LoadedColumnsLevel.C, page.GetValueOrDefault(1), itensPerPage),
                 enabled: enabled,
                order: order.HasValue ?
                    new List<Category.OrderView>() { (Category.OrderView)order.Value } :
                    null,
                parentCategoryId: parentCategoryId,
                parentCategoryFromPath: null,
                name: name);

            if(_exportToExcel && data.Items.Any())
            {
                await ExcelExportData(data.Items);
                // Clear items to do not show in view a lot of data no paging
                data.Items = Enumerable.Empty<Category>();
            }

            ViewData["page"] = data.PageNumber;
            ViewData["pageSize"] = data.PageSize;
            ViewData["totalItems"] = data.TotalItems;
            ViewData["totalPages"] = data.TotalPages;

            return View(data.Items);
        }

        private async Task ExcelExportData(IEnumerable<Category> data)
        {
            try
            {
                var catToViewModel = data.Select(x => new CategoryViewModel()
                {
                    Name = x.NameView,
                    Order = (int)x.Order,
                    Path = x.PathView,
                    Enabled = x.Enabled,
                    Url = x.Url
                }).ToList();

                var dataToTable = Data.Utility.ConvertListToDataTable(catToViewModel);
                DateTime currentDate = DateTime.Now;

                await Data.Utility.ExportDataTableToExcelAsync(dataToTable);

                ViewData["successMsg"] = "Pronto! Disponível em seu repositório Downloads.";                
            }
            catch (Exception ex)
            {
                await DoExcelExportException(ex);
            }
            
        }
        private async Task DoExcelExportException(Exception ex)
        {
            string userId = null;
            var user = await _userManager.GetUserAsync(User);

            ViewData["errorMsg"] = ex.Message;
            if (user != null)
            {
                userId = user.Id;
            }

            await _context.InsertErrorLog(new ErrorLogs()
            {
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                Language = CultureInfo.CurrentCulture.Name,
                Message = ex.Message,
                RequestMethod = "GET",
                RequestUrl = HttpContext.Request.GetEncodedUrl(),
                StackTrace = ex.StackTrace,
                Title = "18052023-CategoryAdmExportToExcel",
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString(),
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetCategoriesByOrder(int order)
        {
            Category.OrderView orderView = (Category.OrderView)order;

            var result = await _context.GetCategories(new Optimization(Optimization.LoadedColumnsLevel.C, 1), true, new List<Category.OrderView>() {
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

                category.Name = category.Name.Trim();
                category.Description = category.Description?.Trim();

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
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,DateCreated,Name,Description,Url,Enabled,Order,ParentCategoryId,Path,Index,Guid")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                category.Name = category.Name.Trim();
                category.Description = category.Description?.Trim();

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
