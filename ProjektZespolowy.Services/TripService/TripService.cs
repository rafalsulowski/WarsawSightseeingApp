using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.DataAccess.Repository;
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
    public class TripService : ITripService
    {
        private readonly ITripRepository _TripRepository;
        public TripService(ITripRepository TripRepository)
        {
            _TripRepository = TripRepository;
        }
        public async Task<RepositoryResponse<int>> CreateTrip(Trip Trip)
        {
            Trip = _TripRepository.Add(Trip);
            RepositoryResponse<bool> res = await _TripRepository.SaveChangesAsync();
            RepositoryResponse<int> response = new RepositoryResponse<int> { 
                Message = res.Message,
                Data = Trip.Id,
                Success = res.Success
            };
            return response;
        }

        public async Task<RepositoryResponse<bool>> DeleteTrip(Trip Trip)
        {
            foreach(var el in Trip.Places)
                _TripRepository.DeletePlaceFromTrip(el);

            _TripRepository.Remove(Trip);
            return await _TripRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeleteTripLike(int userId, int TripId)
        {
            //_TripRepository.DeleteTripLike(userId, TripId);
            return await _TripRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<Trip>> GetTripAsync(Expression<Func<Trip, bool>> filter, string? includeProperties = null)
        {
            return await _TripRepository.GetFirstOrDefault(filter, includeProperties);
        }

        public async Task<RepositoryResponse<List<Trip>>> GetTripsAsync(Expression<Func<Trip, bool>>? filter = null, string? includeProperties = null)
        {
            return await _TripRepository.GetAll(filter, includeProperties);
        }

        public async Task<RepositoryResponse<bool>> LikeTrip(TripLikes like)
        {
           // _TripRepository.LikeTrip(like);
            return await _TripRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> UpdateTrip(TripDTO Trip)
        {
            var tripDb = await _TripRepository.GetFirstOrDefault(u => u.Id == Trip.Id);
            if( tripDb.Data == null )
            {
                return new RepositoryResponse<bool> { Success = false, Message = "Nie ma takiego Tripa" };
            }
            var trip = tripDb.Data;
            trip.Title = Trip.Title;
            trip.Description = Trip.Description;
            trip.Author = Trip.Author;
            trip.StartHour = Trip.StartHour;
            trip.StopHour = Trip.StopHour;
            trip.Date = Trip.Date;
            trip.TransportType = Trip.TransportType;
            trip.Budget = Trip.Budget;
            trip.IsPublic = Trip.IsPublic;
            var response = await _TripRepository.Update(trip);
            if (response.Success == false)
            {
                return response;
            }
            response = await _TripRepository.SaveChangesAsync();
            return response;
        }


        public async Task<RepositoryResponse<bool>> AddPlaceToTrip(PlacesTrips place)
        {
            await _TripRepository.AddPlaceToTrip(place);
            return await _TripRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeletePlaceFromTrip(PlacesTrips place)
        {
            await _TripRepository.DeletePlaceFromTrip(place);
            return await _TripRepository.SaveChangesAsync();
        }
    }
}
