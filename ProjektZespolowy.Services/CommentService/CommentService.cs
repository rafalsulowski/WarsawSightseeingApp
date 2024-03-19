using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.DataAccess.Repository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<RepositoryResponse<Comment>> GetCommentAsync(Expression<Func<Comment, bool>> filter, string? includeProperties = null)
        {
            return await _commentRepository.GetFirstOrDefault(filter, includeProperties);
        }

        public async Task<RepositoryResponse<bool>> CreateComment(Comment Comment)
        {
            _commentRepository.Add(Comment);
            return await _commentRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> LikeComment(CommentLikes like)
        {
            _commentRepository.LikeComment(like);
            return await _commentRepository.SaveChangesAsync();
        }


        public async Task<RepositoryResponse<bool>> DeleteLikeCommentPost(int userId, int commentId)
        {
            _commentRepository.DeleteLikeCommentPost(userId, commentId);
            return await _commentRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeleteComment(Comment Comment)
        {
            foreach (var likeComment in Comment.Likes)
            {
                _commentRepository.DeleteLikeCommentPost(likeComment.UserLikeId, likeComment.CommentLikeId);
            }

            _commentRepository.Remove(Comment);
            return await _commentRepository.SaveChangesAsync();
        }
    }
}
