using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.CommentPlaceService
{
    public interface ICommentPlaceService
    {
        Task<RepositoryResponse<List<CommentPlace>>> GetCommentPlacesAsync(Expression<Func<CommentPlace, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<CommentPlace>> GetCommentPlaceAsync(Expression<Func<CommentPlace, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> CreateCommentPlace(CommentPlace CommentPlace);
        Task<RepositoryResponse<bool>> UpdateCommentPlace(CommentPlace CommentPlace);
        Task<RepositoryResponse<bool>> DeleteCommentPlace(CommentPlace CommentPlace);
        Task<RepositoryResponse<bool>> LikeComment(CommentPlaceLikes like);
    }
}
