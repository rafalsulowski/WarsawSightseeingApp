using Microsoft.EntityFrameworkCore;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<RepositoryResponse<bool>> Update(Post post);
        void LikePost(PostLikes like);
        void DeletePostLike(int userId, int postId);
    }
}
