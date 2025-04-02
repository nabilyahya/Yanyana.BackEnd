using Microsoft.AspNetCore.Mvc;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await _categoryManager.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var createdCategory = await _categoryManager.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {

            var result = await _categoryManager.UpdateCategoryAsync(id, category);
            if (!result)
            {
                return BadRequest(); 
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryManager.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
