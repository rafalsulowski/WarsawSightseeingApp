using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.DataAccess.Repository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Services.CommentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentService _commentService;
        public PostService(IPostRepository postRepository, ICommentService commentService)
        {
            _postRepository = postRepository;
            _commentService = commentService;
        }
        public async Task<RepositoryResponse<bool>> CreatePost(Post Post)
        {
            _postRepository.Add(Post);
            return await _postRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeletePost(Post Post)
        {
            for(int i = 0; i < Post.Comments.Count; i++)
            {
                _commentService.DeleteComment(Post.Comments[i]);
            }
            for(int i = 0; i < Post.Likes.Count; i++)
            {
                _postRepository.DeletePostLike(Post.Likes[i].UserLikeId, Post.Likes[i].PostLikeId);
            }

            var postDB = _postRepository.GetFirstOrDefault(u => u.Id == Post.Id).Result;
            _postRepository.Remove(postDB.Data);

            return await _postRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeletePostLike(int userId, int postId)
        {
            _postRepository.DeletePostLike(userId, postId);
            return await _postRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<Post>> GetPostAsync(Expression<Func<Post, bool>> filter, string? includeProperties = null)
        {
            return await _postRepository.GetFirstOrDefault(filter, includeProperties);
        }

        public async Task<RepositoryResponse<List<Post>>> GetPostsAsync(Expression<Func<Post, bool>>? filter = null, string? includeProperties = null)
        {
            return await _postRepository.GetAll(filter, includeProperties);
        }

        public async Task<RepositoryResponse<bool>> LikePost(PostLikes like)
        {
            _postRepository.LikePost(like);
            return await _postRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> UpdatePost(Post Post)
        {
            var response = await _postRepository.Update(Post);
            if (response.Success == false)
            {
                return response;
            }
            response = await _postRepository.SaveChangesAsync();
            return response;
        }
    }
}
