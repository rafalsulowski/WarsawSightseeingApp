using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProjektZespolowy.DataAccess;
using ProjektZespolowy.DataAccess.Data;
using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.DataAccess.Repository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;

namespace ProjektZespolowy.Services.PlaceService
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _PlaceRepository;
        public PlaceService(IPlaceRepository PlaceRepository)
        {
            _PlaceRepository = PlaceRepository;
        }

        public async Task<RepositoryResponse<bool>> CreatePlace(Place Place)
        {
            _PlaceRepository.Add(Place);
            var response = await _PlaceRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<bool>> DeletePlace(Place Place)
        {
            _PlaceRepository.Remove(Place);
            var response = await _PlaceRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<Place>> GetPlaceAsync(Expression<Func<Place, bool>> filter, string? includeProperties = null)
        {
            var response = await _PlaceRepository.GetFirstOrDefault(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<List<Place>>> GetPlacesAsync(Expression<Func<Place, bool>>? filter = null, string? includeProperties = null)
        {
            var response = await _PlaceRepository.GetAll(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<bool>> LikePlace(PlaceLikes like)
        {
            _PlaceRepository.LikePlace(like);
            return await _PlaceRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeletePlaceLike(int userId, int PlaceId)
        {
            _PlaceRepository.DeletePlaceLike(userId, PlaceId);
            return await _PlaceRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> UpdatePlace(Place Place)
        {
            var response = await _PlaceRepository.Update(Place);
            if(response.Success==false)
            {
                return response;
            }
            response = await _PlaceRepository.SaveChangesAsync();
            return response;
        }
    }
}
