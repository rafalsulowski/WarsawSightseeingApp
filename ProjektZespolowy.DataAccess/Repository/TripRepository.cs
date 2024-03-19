using Microsoft.EntityFrameworkCore;
using ProjektZespolowy.DataAccess.Data;
using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.Repository
{
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        private ApplicationDbContext _context;
        public TripRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RepositoryResponse<bool>> Update(Trip trip)
        {
            var tripDB = await GetFirstOrDefault(u => u.Id == trip.Id);
            if (tripDB == null)
            {
                return new RepositoryResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Trip with this Id was not found."
                };
            }
            _context.Trips.Attach(trip);
            _context.Entry(trip).State = EntityState.Modified;
            return new RepositoryResponse<bool> { Data = true };
        }

        public async Task<RepositoryResponse<bool>> AddPlaceToTrip(PlacesTrips placesTrips)
        {
            var placeTrip = _context.PlacesTrips.FirstOrDefault(u => u.PlaceId == placesTrips.PlaceId && u.TripId == placesTrips.TripId);

            var trip = _context.Trips.FirstOrDefault(u => u.Id == placesTrips.TripId);
            var place = _context.Places.FirstOrDefault(u => u.Id == placesTrips.PlaceId);
            placesTrips.Trip = trip;
            placesTrips.Place = place;

            if (placeTrip == null)
            {
                _context.PlacesTrips.Add(placesTrips);
            }
            else
            {
                _context.PlacesTrips.Attach(placesTrips);
                _context.Entry(placesTrips).State = EntityState.Modified;
            }
            return new RepositoryResponse<bool> { Data = true };
        }
        public async Task<RepositoryResponse<bool>> DeletePlaceFromTrip(PlacesTrips placesTrips)
        {
            var placeTrip = _context.PlacesTrips.FirstOrDefault(u => u.PlaceId == placesTrips.PlaceId && u.TripId == placesTrips.TripId);

            if (placeTrip != null)
            {
                _context.PlacesTrips.Remove(placeTrip);
            }

            return new RepositoryResponse<bool> { Data = true };
        }
    }
}
