using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<RepositoryResponse<bool>> Update(Comment comment);
        void LikeComment(CommentLikes like);
        void DeleteLikeCommentPost(int userId, int commentId);
    }
}
