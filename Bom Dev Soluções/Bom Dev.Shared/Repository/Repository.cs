using Data.Context;
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

        public async Task<IEnumerable<Category>> GetCategories(Optimization op = null, bool? enabled = null, List<Category.OrderView> order = null, int? parentCategoryId = null, string parentCategoryFromPath = null)
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

            switch (op?.LoadedColumns)
            {
                case Optimization.LoadedColumnsLevel.A:
                    query = from s in query
                            join t in _context.TranslationObject
                            on new { ObjectGuid = s.Guid, Language = _currentLanguage } equals new { t.ObjectGuid, t.Language } into translations
                            from translation in translations.DefaultIfEmpty()
                            orderby s.Path
                            select new Category(translation == null ? s.Name : translation.Value)
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
                            };

                    break;

                case Optimization.LoadedColumnsLevel.B:
                    query = from s in query
                            join t in _context.TranslationObject
                            on new {ObjectGuid = s.Guid, Language = _currentLanguage } equals new { t.ObjectGuid, t.Language} into translations
                            from translation in translations.DefaultIfEmpty()
                            orderby s.Path
                            select new Category(translation == null ? s.Name : translation.Value)
                            {
                                CategoryId = s.CategoryId,
                                Name = s.Name,                                
                                Order = s.Order,
                                Path = s.Path,
                                DateCreated = s.DateCreated,
                                ParentCategoryId = s.ParentCategoryId,
                                Url = s.Url
                            };                                               

                    break;

                case Optimization.LoadedColumnsLevel.C:

                    query = from s in query
                            join t in _context.TranslationObject
                            on new { ObjectGuid = s.Guid, Language = _currentLanguage } equals new { t.ObjectGuid, t.Language } into translations
                            from translation in translations.DefaultIfEmpty()
                            orderby s.Path
                            select new Category(translation == null ? s.Name : translation.Value)
                            {
                                CategoryId = s.CategoryId,
                                Name = s.Name,                                
                                Order = s.Order,
                                Path = s.Path,
                                Url = s.Url
                            };

                    break;
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
                category.Guid = Guid.NewGuid();

                await _context.Set<Category>().AddAsync(category);

                // If has others languages
                if (Utility.IsValidJson(category.Name))
                {
                    try
                    {
                        Dictionary<string, string> translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(category.Name);

                        foreach(var lang in translations)
                        {
                            await SetTranslationObject(new TranslationObject()
                            {
                                CreatedDate = DateTime.Now,
                                Language = lang.Key,
                                Value = lang.Value,
                                ObjectGuid = category.Guid                                
                            });
                        }                        
                    }
                    catch (Exception ex) {
                        await InsertErrorLog(new ErrorLogs()
                        {
                            IpAddress = null,
                            Language = _currentLanguage,
                            Message = ex.Message,
                            RequestMethod = null,
                            RequestUrl = null,
                            StackTrace = ex.StackTrace,
                            Title = "15052023-InvalidTranslateJSONParse",
                            UserAgent = null,
                            UserId = null
                        });
                    }
                }
                else
                {
                    await DeleteTranslationObject(category.Guid);
                }

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

                // If has others languages
                if (Utility.IsValidJson(category.Name))
                {
                    try
                    {
                        Dictionary<string, string> translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(category.Name);

                        foreach (var lang in translations)
                        {
                            await SetTranslationObject(new TranslationObject()
                            {
                                CreatedDate = DateTime.Now,
                                Language = lang.Key,
                                Value = lang.Value,
                                ObjectGuid = category.Guid
                            });
                        }
                    }
                    catch(Exception ex) {
                        
                    }
                }
                else
                {
                    await DeleteTranslationObject(category.Guid);
                }

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

        #region TranslationObject
        public async Task<IEnumerable<TranslationObject>> GetTranslationObject(Guid objectGuid, string language = null)
        {
            bool languageHasValue = !string.IsNullOrWhiteSpace(language);

            var result = from x in _context.Set<TranslationObject>()?.AsNoTracking()
                         where x.ObjectGuid == objectGuid &&
                         (!languageHasValue || x.Language.ToUpper() == language.ToUpper())
                         select x;

            return await result.ToListAsync();            
        }



        public async Task DeleteTranslationObject(Guid objectGuid, string language = null)
        {
            var obj = await GetTranslationObject(objectGuid, language);

            if(obj != null && obj.Any())
            {
                _context.Set<TranslationObject>().RemoveRange(obj);

                await _context.SaveChangesAsync();
            }            
        }

        public async Task SetTranslationObject(TranslationObject translationObject)
        {
            if(translationObject != null)
            {
                // Remove old translations
                await DeleteTranslationObject(translationObject.ObjectGuid, translationObject.Language);

                // Add new
                _context.Set<TranslationObject>().Add(translationObject);

                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region ErrorLogs
        public async Task InsertErrorLog([NotNullAttribute] ErrorLogs errorLog)
        {
            await _context.Set<ErrorLogs>().AddAsync(errorLog);
        }
        #endregion
    }
}
