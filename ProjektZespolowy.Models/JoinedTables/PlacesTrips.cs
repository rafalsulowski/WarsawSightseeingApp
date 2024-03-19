using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.JoinedTables
{
    public class PlacesTrips
    {
        public int PlaceId { get; set; }
        public int TripId { get; set; }
        public Place Place { get; set; } = new Place();
        public Trip Trip { get; set; } = new Trip();
        public int Sequence { get; set; }
        public int TimeForPlace { get; set; }
        public int BudgetForPlace { get; set; }

        public PlacesTripsDTO MapToDTO()
        {
            PlacesTripsDTO placesTripsDTO = new PlacesTripsDTO();
            placesTripsDTO.PlaceId = PlaceId;
            placesTripsDTO.TripId = TripId;
            placesTripsDTO.Sequence = Sequence;
            placesTripsDTO.TimeForPlace = TimeForPlace;
            placesTripsDTO.BudgetForPlace = BudgetForPlace;
            return placesTripsDTO;
        }
    }
}
