using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.CommentService
{
    public interface ICommentService
    {
        Task<RepositoryResponse<Comment>> GetCommentAsync(Expression<Func<Comment, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> CreateComment(Comment Comment);
        Task<RepositoryResponse<bool>> LikeComment(CommentLikes like);
        Task<RepositoryResponse<bool>> DeleteComment(Comment Comment);
        Task<RepositoryResponse<bool>> DeleteLikeCommentPost(int userId, int commentId);
    }
}
