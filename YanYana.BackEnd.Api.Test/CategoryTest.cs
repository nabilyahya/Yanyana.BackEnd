using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Yanyana.BackEnd.Api.Controllers;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Api.Test
{
    [Collection("Yanyana")]
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryManager> _mockCategoryManager;

        public CategoriesControllerTests()
        {
            _mockCategoryManager = new Mock<ICategoryManager>();
        }

        [Fact]
        public async Task GetCategories_ReturnsAllCategories()
        {
            // Arrange
            var testCategories = new List<Category>
            {
                new Category { CategoryId = 1, Name = "Test Category 1", Description = "Description 1" },
                new Category { CategoryId = 2, Name = "Test Category 2", Description = "Description 2" }
            };

            _mockCategoryManager.Setup(m => m.GetAllCategoriesAsync())
                .ReturnsAsync(testCategories);
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<Category>>>(result);
            var categories = Assert.IsAssignableFrom<List<Category>>(okResult.Result);
            Assert.Equal(2, categories.Count);
        }

        [Fact]
        public async Task GetCategory_ExistingId_ReturnsCategory()
        {
            // Arrange
            var testCategory = new Category { CategoryId = 1, Name = "Test Category", Description = "Description" };
            _mockCategoryManager.Setup(m => m.GetCategoryByIdAsync(1))
                .ReturnsAsync(testCategory);
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.GetCategory(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            var category = okResult.Value as Category;

            Assert.NotNull(category);
            Assert.Equal("Test Category", category.Name);
        }

        [Fact]
        public async Task GetCategory_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockCategoryManager.Setup(m => m.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Category)null);
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.GetCategory(999);
            var okResult = result.Result as OkObjectResult;
            var category = okResult.Value as Category;
            Assert.Null(category);
        }

        [Fact]
        public async Task PostCategory_ValidCategory_ReturnsCreated()
        {
            // Arrange
            var newCategory = new Category { Name = "New Category", Description = "New Description" };
            _mockCategoryManager.Setup(m => m.CreateCategoryAsync(It.IsAny<Category>()))
           .ReturnsAsync((Category c) =>
           {
               c.CategoryId = 1;  // Simulate DB assigning an ID
               return c;
           });
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.CreateCategory(newCategory);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var category = Assert.IsType<Category>(createdResult.Value);
            // Assert
            Assert.Equal("New Category", category.Name);
            Assert.Equal("New Description", category.Description);
        }

        [Fact]
        public async Task PutCategory_ValidCategory_ReturnsNoContent()
        {
            // Arrange
            var updatedCategory = new Category
            {
                CategoryId = 1,
                Name = "Updated Name",
                Description = "Updated Description"
            };

            _mockCategoryManager.Setup(m => m.UpdateCategoryAsync(It.IsAny<int>(), It.IsAny<Category>()))
           .Returns(Task.FromResult(true));

            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.UpdateCategory(1, updatedCategory);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCategory_NonMatchingId_ReturnsBadRequest()
        {
            // Arrange
            var updatedCategory = new Category { CategoryId = 2, Name = "Updated Name", Description = "Updated Description" };
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.UpdateCategory(1, updatedCategory);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ExistingId_ReturnsNoContent()
        {
            // Arrange
            _mockCategoryManager.Setup(m => m.DeleteCategoryAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var controller = new CategoryController(_mockCategoryManager.Object);

            // Act
            var result = await controller.DeleteCategory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
