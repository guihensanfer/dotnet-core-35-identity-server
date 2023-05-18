using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interface
{
    public interface IRepository
    {
        #region Category
        public Task<int> InsertCategory([NotNullAttribute] Category category);
        public Task UpdateCategory([NotNullAttribute] Category category);
        public Task DeleteCategory(int categoryId);
        public Task<IEnumerable<Category>> GetCategories(Optimization op = null, bool? enabled = null, List<Category.OrderView> order = null, int? parentCategoryId = null, string parentCategoryFromPath = null, string name = null);        
        public Task<Category> GetCategoryById(int categoryId);
        public Task<bool> ExistsParentCategory(int categoryId);
        public Task<bool> ExistsCategoryByName(string name);
        #endregion

        #region CacheObject
        public Task<CacheObject> GetCacheObject(string key, string language, int? cacheObjectId = null);        
        public Task<int> SetCacheObject([NotNullAttribute] CacheObject cacheObject);
        #endregion

        #region ErrorLogs
        public Task InsertErrorLog([NotNullAttribute]ErrorLogs errorLog);
        #endregion
    }
}
