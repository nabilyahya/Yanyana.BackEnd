using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Yanyana.BackEnd.Api.Controllers;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Data.Context;

namespace Yanyana.BackEnd.Api.Test
{

    public class CommentsControllerTests
    {
        private readonly Mock<ICommentManager> _mockManager;

        public CommentsControllerTests()
        {
            _mockManager = new Mock<ICommentManager>();
 
        }

        [Fact]
        public async Task GetAllComments_ReturnsOkResult()
        {
            var testComments = new List<Comment>()
            {
                new Comment {CommentId = 1 , IsDeleted = false , Content ="First Comment"},
                new Comment {CommentId = 2 , IsDeleted = false , Content ="Second Comment"},
            };
            _mockManager.Setup(m => m.GetAllCommentsAsync()).ReturnsAsync(testComments);

            var controller = new CommentsController(_mockManager.Object);
            var result = await controller.GetAllComments();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCommentById_ReturnsNotFound_WhenInvalidId()
        {
            _mockManager.Setup(m => m.GetCommentByIdAsync(1)).ReturnsAsync((Comment)null);
            var controller = new CommentsController(_mockManager.Object);
            var result = await controller.GetCommentById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCommentById_ReturnsComment_WhenValidId()
        {
            var comment = new Comment { CommentId = 1 };
            _mockManager.Setup(m => m.GetCommentByIdAsync(1)).ReturnsAsync(comment);
            var controller = new CommentsController(_mockManager.Object);
            var result = await controller.GetCommentById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(comment, okResult.Value);
        }

        [Fact]
        public async Task CreateComment_ReturnsCreatedAtAction()
        {
            var comment = new Comment { CommentId = 1 };
            _mockManager.Setup(m => m.CreateCommentAsync(comment)).Returns(Task.CompletedTask);
            var controller = new CommentsController(_mockManager.Object);
            var result = await controller.CreateComment(comment);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetCommentById", createdAtResult.ActionName);
            Assert.Equal(1, createdAtResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateComment_ReturnsBadRequest_WhenIdMismatch()
        {
            var controller = new CommentsController(_mockManager.Object);
            var result = await controller.UpdateComment(2, new Comment { CommentId = 1 });

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteComment_ReturnsNoContent()
        {
            _mockManager.Setup(m => m.DeleteCommentAsync(1)).Returns(Task.CompletedTask);
            var controller = new CommentsController(_mockManager.Object);

            var result = await controller.DeleteComment(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}