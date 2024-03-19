using Microsoft.EntityFrameworkCore;
using ProjektZespolowy.DataAccess.Data;
using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<bool>> Update(Post post)
        {
            var postDB = await GetFirstOrDefault(u => u.Id == post.Id);
            if (postDB == null)
            {
                return new RepositoryResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Post with this Id was not found."
                };
            }
            _context.Posts.Attach(post);
            _context.Entry(post).State = EntityState.Modified;
            return new RepositoryResponse<bool> { Data = true };
        }
        public void LikePost(PostLikes like)
        {
            var likeDB = _context.PostLikes.FirstOrDefault(u => u.PostLikeId == like.PostLikeId && u.UserLikeId == like.UserLikeId);
            if(likeDB == null)
            {
                _context.PostLikes.Add(like);
            } else
            {
                likeDB.IsLike = like.IsLike;
                _context.PostLikes.Attach(likeDB);
                _context.Entry(likeDB).State = EntityState.Modified;
            }
        }

        public void DeletePostLike(int userId, int postId)
        {
            var likeDB = _context.PostLikes.FirstOrDefault(u => u.PostLikeId == postId && u.UserLikeId == userId);
            if(likeDB != null)
            {
                if (_context.Entry(likeDB).State == EntityState.Detached)
                {
                    _context.PostLikes.Attach(likeDB);
                }
                _context.PostLikes.Remove(likeDB);
            }
        }
    }
}
