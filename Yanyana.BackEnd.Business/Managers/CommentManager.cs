using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Data.Context;
using System.Transactions;
using static Yanyana.BackEnd.Business.Managers.CommentManager;

namespace Yanyana.BackEnd.Business.Managers
{
    public interface ICommentManager
    {
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment updatedComment);
        Task DeleteCommentAsync(int commentId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<IEnumerable<Comment>> GetCommentsByPlaceIdAsync(int placeId);
    }
    public class CommentManager : ICommentManager
    {
        private readonly YanDbContext _context;
  
        public CommentManager(YanDbContext context)
        {
            _context = context;
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            ValidateComment(comment);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    comment.CreatedDate = DateTime.UtcNow;
                    comment.ModifiedDate = DateTime.UtcNow;
                    comment.IsDeleted = false;

                    await _context.Comments.AddAsync(comment);
                    int saveResult = await _context.SaveChangesAsync();

                    if (saveResult <= 0)
                        throw new InvalidOperationException("Failed to save comment");

                    transaction.Complete();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                throw new InvalidOperationException("Database error occurred while creating comment", ex);
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while creating comment", ex);
            }
        }

        public async Task UpdateCommentAsync(Comment updatedComment)
        {
            if (updatedComment == null)
                throw new ArgumentNullException(nameof(updatedComment));

            ValidateComment(updatedComment);

            try
            {
                var existingComment = await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Place)
                    .Include(c => c.CommentPictures)
                    .FirstOrDefaultAsync(c => c.CommentId == updatedComment.CommentId);

                if (existingComment == null)
                    throw new KeyNotFoundException($"Comment with ID {updatedComment.CommentId} not found");

                existingComment.Content = updatedComment.Content;
                existingComment.ModifiedDate = DateTime.UtcNow;

                int saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                    throw new InvalidOperationException("Failed to update comment");

            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the concurrency exception details
                throw new InvalidOperationException("Concurrency error occurred while updating comment", ex);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                throw new InvalidOperationException("Database error occurred while updating comment", ex);
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while updating comment", ex);
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            try
            {
                var comment = await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Place)
                    .Include(c => c.CommentPictures)
                    .FirstOrDefaultAsync(c => c.CommentId == commentId);

                if (comment == null)
                    throw new KeyNotFoundException($"Comment with ID {commentId} not found");

                comment.IsDeleted = true;
                comment.ModifiedDate = DateTime.UtcNow;

                int saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                    throw new InvalidOperationException("Failed to delete comment");

            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                throw new InvalidOperationException("Database error occurred while deleting comment", ex);
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while deleting comment", ex);
            }
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            try
            {
                return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Place)
                    .Include(c => c.CommentPictures)
                    .FirstOrDefaultAsync(c => c.CommentId == commentId && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while retrieving comment", ex);
            }
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            try
            {
                return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Place)
                    .Include(c => c.CommentPictures)
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while retrieving comments", ex);
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPlaceIdAsync(int placeId)
        {
            try
            {
                return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Place)
                    .Include(c => c.CommentPictures)
                    .Where(c => c.PlaceId == placeId && !c.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception details
                throw new Exception("An unexpected error occurred while retrieving comments by place ID", ex);
            }
        }

        private void ValidateComment(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Comment content cannot be empty or whitespace");

            if (comment.UserId <= 0)
                throw new ArgumentException("Invalid user ID");

            if (comment.PlaceId <= 0)
                throw new ArgumentException("Invalid place ID");
        }
    }
}