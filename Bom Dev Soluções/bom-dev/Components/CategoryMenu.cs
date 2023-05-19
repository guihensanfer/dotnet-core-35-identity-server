using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Data.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Bom_Dev.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly Data.Interface.IRepository _context;        

        public CategoryMenu(Data.Interface.IRepository context)
        {
            _context = context;
        }                

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string defaultKey = CacheObject.Objects.MenuItensSite.ToString();
            var cache = await _context.GetCacheObject(defaultKey, System.Globalization.CultureInfo.CurrentCulture.Name);

            if (cache == null)
            {
                cache = new CacheObject(defaultKey);
                cache.Expiration = DateTime.Now.AddDays(7);
            }                

            if (string.IsNullOrWhiteSpace(cache.Value))
            {
                var categories = await _context.GetCategories(new Optimization(Optimization.LoadedColumnsLevel.A, 1), true,
                    new List<Category.OrderView>() { 
                        Category.OrderView.First,
                        Category.OrderView.Second
                    });
                var categoriesFirstOrder = categories.Items.Where(x => x.Order == Category.OrderView.First)
                    .OrderBy(x => x.Index);
                StringBuilder sb = new StringBuilder();
                
                // first order
                foreach(var c1 in categoriesFirstOrder)
                {
                    var categoriesSecondOrder = categories.Items
                         .Where(x => x.ParentCategoryId == c1.CategoryId)
                         .OrderBy(x => x.Index);

                    sb.Append("<div class=\"dropdown\" style=\"margin-left:1px\">");
                    if (string.IsNullOrEmpty(c1.Url))
                    {
                        sb.AppendFormat("  <button class=\"btn btn-secondary dropdown-toggle\" type=\"button\" id=\"dropdownMenuButton{0}\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">", c1.CategoryId);
                        sb.Append(c1.NameView);
                        sb.Append("  </button>");
                    }                    
                    else
                    {
                        sb.AppendFormat("  <a href=\"{1}\" class=\"btn btn-secondary\">", c1.CategoryId, c1.Url);
                        sb.Append(c1.NameView);
                        sb.Append("  </a>");
                    }
                    sb.Append("  <div class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuButton\">");


                    sb.Append("<nav aria-label=\"breadcrumb\" class=\"menuBreadcrumb\">");
                    sb.Append("<ol class=\"breadcrumb\">");
                    sb.AppendFormat("<li class=\"breadcrumb-item active\">{0}</li>", c1.NameView);
                    sb.Append("</ol>");
                    sb.Append("</nav>");                                           

                    // second order
                    foreach (var c2 in categoriesSecondOrder)
                    {
                        if (string.IsNullOrEmpty(c2.Url))
                        {
                            sb.AppendFormat("    <a class=\"dropdown-item\" onclick=\"menuShowCategories('{1}', this)\" href=\"#\">{0}</a>",
                            c2.NameView,
                            c2.Path);
                        }
                        else
                        {
                            sb.AppendFormat("    <a class=\"dropdown-item\" href=\"{1}\">{0}</a>",
                            c2.NameView,
                            c2.Url);
                        }                        
                    }

                    sb.Append("  </div>");
                    sb.Append("</div>");                    
                }                

                cache.Value = sb.ToString();
                await _context.SetCacheObject(cache);
            }            

            return new HtmlContentViewComponentResult(new HtmlString(cache.Value));
        }              
    }
}
