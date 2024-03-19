using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.TripService
{
    public interface ITripService
    {
        Task<RepositoryResponse<List<Trip>>> GetTripsAsync(Expression<Func<Trip, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<Trip>> GetTripAsync(Expression<Func<Trip, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<int>> CreateTrip(Trip Trip);
        Task<RepositoryResponse<bool>> UpdateTrip(TripDTO Trip);
        Task<RepositoryResponse<bool>> DeleteTrip(Trip Trip);
        Task<RepositoryResponse<bool>> LikeTrip(TripLikes like);
        Task<RepositoryResponse<bool>> DeleteTripLike(int userId, int TripId);
        Task<RepositoryResponse<bool>> AddPlaceToTrip(PlacesTrips place);
        Task<RepositoryResponse<bool>> DeletePlaceFromTrip(PlacesTrips place);
    }
}
