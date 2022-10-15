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
        public Task<IEnumerable<Category>> GetCategories(Optimization op = null, bool ? enabled = null, Category.OrderView? order = null);        
        public Task<Category> GetCategoryById(int categoryId);
        public Task<bool> ExistsParentCategory(int categoryId);
        public Task<bool> ExistsCategoryByName(string name);
        #endregion        
    }
}
