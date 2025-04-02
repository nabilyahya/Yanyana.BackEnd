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
        Task<bool> UpdateCategoryAsync(int id ,Category category);
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
        public async Task<Category?> CreateCategoryAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            bool categoryExists = await _yanContext.Categories
                .AnyAsync(c => c.Name == category.Name);

            if (categoryExists)
            {
                return null; 
            }

            _yanContext.Categories.Add(category);
            await _yanContext.SaveChangesAsync();

            return category;
        }
        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            if (category.CategoryId != id)
            {
                return false; 
            }

            var existingCategory = await _yanContext.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return false; 
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _yanContext.Categories.Update(existingCategory);
            await _yanContext.SaveChangesAsync();
            return true;
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
