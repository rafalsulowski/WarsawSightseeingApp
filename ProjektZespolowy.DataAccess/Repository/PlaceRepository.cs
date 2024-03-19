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
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {
        private ApplicationDbContext _context;
        public PlaceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<bool>> Update(Place place)
        {
            var placeDb = await GetFirstOrDefault(u => u.Id == place.Id);
            if (placeDb == null)
            {
                return new RepositoryResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Place with this Id was not found."
                };
            }
            _context.Places.Attach(place);
            _context.Entry(place).State = EntityState.Modified;
            _context.SaveChanges();
            return new RepositoryResponse<bool> { Data = true };
        }

        public void LikePlace(PlaceLikes like)
        {
            var likeDB = _context.PlaceLikes.FirstOrDefault(u => u.PlaceLikeId == like.PlaceLikeId && u.UserLikeId == like.UserLikeId);
            if (likeDB == null)
            {
                _context.PlaceLikes.Add(like);
            }
            else
            {
                likeDB.IsLike = like.IsLike;
                _context.PlaceLikes.Attach(likeDB);
                _context.Entry(likeDB).State = EntityState.Modified;
            }
        }

        public void DeletePlaceLike(int userId, int PlaceId)
        {
            var likeDB = _context.PlaceLikes.FirstOrDefault(u => u.PlaceLikeId == PlaceId && u.UserLikeId == userId);
            if (likeDB != null)
            {
                if (_context.Entry(likeDB).State == EntityState.Detached)
                {
                    _context.PlaceLikes.Attach(likeDB);
                }
                _context.PlaceLikes.Remove(likeDB);
            }
        }
    }
}
