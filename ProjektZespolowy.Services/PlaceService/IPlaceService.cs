using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;

namespace ProjektZespolowy.Services.PlaceService
{
    public interface IPlaceService
    {
        Task<RepositoryResponse<List<Place>>> GetPlacesAsync(Expression<Func<Place, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<Place>> GetPlaceAsync(Expression<Func<Place, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> CreatePlace(Place Place);
        Task<RepositoryResponse<bool>> UpdatePlace(Place Place);
        Task<RepositoryResponse<bool>> DeletePlace(Place Place);
        Task<RepositoryResponse<bool>> LikePlace(PlaceLikes like);
        Task<RepositoryResponse<bool>> DeletePlaceLike(int userId, int PlaceId);
    }
}
