using Data.Context;
using Data.Interface;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using static Data.Models.Optimization;

namespace Data.Repository
{
    public class Repository : IRepository
    {
        private readonly IdentityDbContext _context;

        public Repository(IdentityDbContext context)
        {
            _context = context;
        }

        #region Category
        public async Task DeleteCategory(int categoryId)
        {
            var obj = await GetCategoryById(categoryId);

            if (obj != null)
            {
                _context.Set<Category>().Remove(obj);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories(Optimization op = null, bool ? enabled = null, Category.OrderView? order = null)
        {
            IQueryable<Category> query = _context.Category;

            if (enabled.HasValue)
            {
                query = query.Where(x => x.Enabled.Equals(enabled.Value));
            }
            if (order.HasValue)
            {
                var orderId = (int)order.Value;

                query = query.Where(x => ((int)x.Order).Equals(orderId));
            }

            if (op != null)
            {
                switch (op.LoadedColumns)
                {
                    case LoadedColumnsLevel.B:
                        query = query.Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order,
                            Path = s.Path,
                            DateCreated = s.DateCreated
                        })
                        .OrderBy(x => x.Path);                        

                        break;

                    case LoadedColumnsLevel.C:
                        query = query.Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order,
                            Path = s.Path
                        })
                        .OrderBy(x => x.Path);

                        break;
                }
            }            

            return await query                
                .AsNoTracking()                
                .ToListAsync();
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await _context.Category
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId.Equals(categoryId));
        }

        public async Task<int> InsertCategory(Category category)
        {
            if (category != null)
            {
                if (category.DateCreated.Equals(default(DateTime)))
                {
                    category.DateCreated = DateTime.Now;
                }

                await _context.Set<Category>().AddAsync(category);

                return await _context.SaveChangesAsync();
            }

            return await Task.FromResult(0);
        }        

        public async Task UpdateCategory(Category category)
        {
            if (category != null)
            {
                var oldCategory = await GetCategoryById(category.CategoryId);
                string oldName = oldCategory.Name;
                string newName = category.Name;

                

                if (!string.Equals(oldName, newName, StringComparison.Ordinal))
                {
                    // if new name, then update path all parents category
                    
                    var parentsCategory = _context.Set<Category>()                        
                        .Where(x =>

                            x.CategoryId != category.CategoryId && // prevent error to update after this code
                            x.Path.Contains(oldName) && // get all contains by name old name category
                            (int)x.Order >= (int)category.Order

                        );                    
                    

                    await parentsCategory.ForEachAsync(x =>
                    {
                        x.Path = x.Path.Replace(oldName, newName);                        
                    });

                    
                }                

                // Update new category data
                _context.Set<Category>().Update(category);

                await _context.SaveChangesAsync();
            }            
        }

        public async Task<bool> ExistsParentCategory(int categoryId)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.ParentCategoryId == categoryId);
        }

        public async Task<bool> ExistsCategoryByName(string name)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Name == name);
        }
        #endregion        
    }
}
