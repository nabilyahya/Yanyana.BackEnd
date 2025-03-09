using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Data.Context;

namespace Yanyana.BackEnd.Business.Managers
{
    public interface ICategoryManager
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
    public class CategoryManager : ICategoryManager
    {
        private readonly YanDbContext _yanContext;
        public CategoryManager(YanDbContext context)
        {
            _yanContext = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _yanContext.Categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _yanContext.Categories.FindAsync(id) ??
             throw new ArgumentNullException(nameof(id), "Category not found.");
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _yanContext.Categories.Add(category);
            await _yanContext.SaveChangesAsync();
            return category;
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));
            // Optionally, you can verify the category exists before updating.
            _yanContext.Categories.Update(category);
            await _yanContext.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _yanContext.Categories.FindAsync(id);
            if (category != null)
            {
                _yanContext.Categories.Remove(category);
                await _yanContext.SaveChangesAsync();
            }
            // Optionally, you might throw an exception or return a status if the category wasn't found.
        }
    }
}
