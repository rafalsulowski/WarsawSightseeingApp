using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.PostService
{
    public interface IPostService
    {
        Task<RepositoryResponse<List<Post>>> GetPostsAsync(Expression<Func<Post, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<Post>> GetPostAsync(Expression<Func<Post, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> CreatePost(Post Post);
        Task<RepositoryResponse<bool>> UpdatePost(Post Post);
        Task<RepositoryResponse<bool>> DeletePost(Post Post);
        Task<RepositoryResponse<bool>> LikePost(PostLikes like);
        Task<RepositoryResponse<bool>> DeletePostLike(int userId, int postId);
    }
}
