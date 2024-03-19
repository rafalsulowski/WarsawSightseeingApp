using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface ICommentPlaceRepository : IRepository<CommentPlace>
    {
        Task<RepositoryResponse<bool>> Update(CommentPlace comment);
        void LikeComment(CommentPlaceLikes like);
    }
}
