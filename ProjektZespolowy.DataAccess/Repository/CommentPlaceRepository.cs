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
    public class CommentPlaceRepository : Repository<CommentPlace>, ICommentPlaceRepository
    {
        private ApplicationDbContext _context;
        public CommentPlaceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<bool>> Update(CommentPlace CommentPlace)
        {
            var CommentPlaceDb = await GetFirstOrDefault(u => u.Id == CommentPlace.Id);
            if (CommentPlaceDb == null)
            {
                return new RepositoryResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "CommentPlace with this Id was not found."
                };
            }
            _context.CommentPlace.Attach(CommentPlace);
            _context.Entry(CommentPlace).State = EntityState.Modified;
            return new RepositoryResponse<bool> { Data = true };
        }

        public void LikeComment(CommentPlaceLikes like)
        {
            var likeDB = _context.CommentPlaceLikes.FirstOrDefault(u => u.CommentPlaceLikeId == like.CommentPlaceLikeId && u.UserLikeId == like.UserLikeId);
            if (likeDB == null)
            {
                _context.CommentPlaceLikes.Add(like);
            }
            else
            {
                likeDB.IsLike = like.IsLike;
                _context.CommentPlaceLikes.Attach(likeDB);
                _context.Entry(likeDB).State = EntityState.Modified;
            }
        }
    }
}
