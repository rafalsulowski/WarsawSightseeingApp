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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<bool>> Update(Comment comment)
        {
            var commentDb = await GetFirstOrDefault(u => u.Id == comment.Id);
            if (commentDb == null)
            {
                return new RepositoryResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Comment with this Id was not found."
                };
            }
            _context.Comments.Attach(comment);
            _context.Entry(comment).State = EntityState.Modified;
            return new RepositoryResponse<bool> { Data = true };
        }

        public void LikeComment(CommentLikes like)
        {
            var likeDB = _context.CommentLikes.FirstOrDefault(u => u.CommentLikeId == like.CommentLikeId && u.UserLikeId == like.UserLikeId);
            if (likeDB == null)
            {
                _context.CommentLikes.Add(like);
            }
            else
            {
                likeDB.IsLike = like.IsLike;
                _context.CommentLikes.Attach(likeDB);
                _context.Entry(likeDB).State = EntityState.Modified;
            }
        }
        public void DeleteLikeCommentPost(int userId, int postId)
        {
            var likeDB = _context.CommentLikes.FirstOrDefault(u => u.CommentLikeId == postId && u.UserLikeId == userId);
            if (likeDB != null)
            {
                if (_context.Entry(likeDB).State == EntityState.Detached)
                {
                    _context.CommentLikes.Attach(likeDB);
                }
                _context.CommentLikes.Remove(likeDB);
            }
        }
    }
}
