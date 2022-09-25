using Data.Context;
using Data.Interface;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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

        public async Task<IEnumerable<Category>> GetCategories(bool? enabled = null, Category.OrderView? order = null, Optimization op = null)
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
                    case LoadedColumnsLevel.C:
                        query = query.Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order
                        }).OrderBy(x => x.Name);

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
                _context.Set<Category>().Update(category);

                await _context.SaveChangesAsync();
            }
        }
        #endregion        
    }
}
