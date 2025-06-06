﻿using Data.Context;
using Data.Interface;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Data.Repository
{
    public class Repository : IRepository
    {
        private readonly IdentityDbContext _context;       
        private string _currentLanguage
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.Name;
            }
        }

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

        public async Task<PagedListResult<Category>> GetCategories(Optimization op, bool? enabled = null, List<Category.OrderView> order = null, int? parentCategoryId = null, string parentCategoryFromPath = null, string name = null)
        {
            IQueryable<Category> query = _context.Set<Category>().AsQueryable();

            if (enabled.HasValue)
            {
                query = query.Where(x => x.Enabled.Equals(enabled.Value));
            }
            if (order != null && order.Any())
            {
                var orderIds = order.Select(x => (int)x);

                query = query.Where(x => orderIds.Contains((int)x.Order));
            }
            if (parentCategoryId.HasValue)
            {
                query = query.Where(x => x.ParentCategoryId == parentCategoryId.Value);
            }
            if (!string.IsNullOrEmpty(parentCategoryFromPath))
            {
                query = query.Where(x => x.Path.StartsWith(parentCategoryFromPath));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            // Count of all possible results without considering filters
            int totalItems = query.AsNoTracking().Count();

            switch (op?.LoadedColumns)
            {
                case Optimization.LoadedColumnsLevel.A:
                    query = query
                        .Skip((op.Page - 1) * op.SizePerPage)
                        .Take(op.SizePerPage)
                        .Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order,
                            Path = s.Path,
                            DateCreated = s.DateCreated,
                            ParentCategoryId = s.ParentCategoryId,
                            Url = s.Url,
                            Index = s.Index,
                            Guid = s.Guid,
                            Enabled = s.Enabled,
                            Description = s.Description
                        });

                    break;

                case Optimization.LoadedColumnsLevel.B:
                    query = query
                        .Skip((op.Page - 1) * op.SizePerPage)
                        .Take(op.SizePerPage)
                        .Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order,
                            Path = s.Path,
                            DateCreated = s.DateCreated,
                            ParentCategoryId = s.ParentCategoryId,
                            Url = s.Url,
                            Enabled = s.Enabled
                        });

                    break;

                case Optimization.LoadedColumnsLevel.C:

                    query = query
                        .Skip((op.Page - 1) * op.SizePerPage)
                        .Take(op.SizePerPage)
                        .Select(s => new Category()
                        {
                            CategoryId = s.CategoryId,
                            Name = s.Name,
                            Order = s.Order,
                            Path = s.Path,
                            Url = s.Url,
                            Enabled = s.Enabled
                        });

                    break;
            }
            
            int totalPages = (int)Math.Ceiling(totalItems / (double)op.SizePerPage);
            var data = await query
                .AsNoTracking()
                .ToListAsync();

            return new PagedListResult<Category>
            {
                Items = data.OrderBy(x => x.Index),
                PageNumber = op.Page,
                PageSize = op.SizePerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            }; 
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
                category.Guid = Guid.NewGuid();

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

        #region CacheObject
        public async Task<CacheObject> GetCacheObject(string key, string language, int? cacheObjectId = null)
        {
            IQueryable<CacheObject> query = _context.CacheObject;

            query = query.Where(x => x.Key == key && x.Language.ToUpper() == language.ToUpper() && x.Expiration >= DateTime.Now);
            
            if (cacheObjectId.HasValue)
            {
                query = query.Where(x => x.CacheObjectId == cacheObjectId.Value);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> SetCacheObject([NotNull] CacheObject cacheObject)
        {
            if (cacheObject != null)
            {
                var current = await GetCacheObject(cacheObject.Key, cacheObject.Language);

                if(current != null)
                {
                    _context.Set<CacheObject>().Remove(current);
                }        
                
                await _context.Set<CacheObject>().AddAsync(cacheObject);

                return await _context.SaveChangesAsync();
            }

            return await Task.FromResult(0);
        }

        #endregion        

        #region ErrorLogs
        public async Task InsertErrorLog([NotNullAttribute] ErrorLogs errorLog)
        {
            await _context.Set<ErrorLogs>().AddAsync(errorLog);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
