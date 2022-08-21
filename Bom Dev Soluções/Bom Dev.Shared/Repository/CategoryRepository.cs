using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CategoryRepository : Interface.ICategory
    {
        private readonly IdentityDbContext _context;

        public CategoryRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var obj = await GetById(id);
            
            _context.Set<Category>().Remove(obj);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> Get()
        {
            return await _context.Set<Category>().AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {            
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task<int> Insert(Category obj)
        {            
            if(obj != null)
            {
                await _context.Set<Category>().AddAsync(obj);

                return await _context.SaveChangesAsync();
            }

            return await Task.FromResult(0);
        }

        public async Task Update(Category obj)
        {
            if (obj != null)
            {
                _context.Set<Category>().Update(obj);

                await _context.SaveChangesAsync();
            }            
        }
    }
}
