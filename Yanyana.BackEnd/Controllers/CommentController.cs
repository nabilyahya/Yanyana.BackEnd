using Microsoft.AspNetCore.Mvc;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentManager _commentManager;

        public CommentsController(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            return Ok(await _commentManager.GetAllCommentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            var comment = await _commentManager.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpGet("place/{placeId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPlaceId(int placeId)
        {
            return Ok(await _commentManager.GetCommentsByPlaceIdAsync(placeId));
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(Comment comment)
        {
            await _commentManager.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.CommentId }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, Comment updatedComment)
        {
            if (id != updatedComment.CommentId) return BadRequest();
            await _commentManager.UpdateCommentAsync(updatedComment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentManager.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
   