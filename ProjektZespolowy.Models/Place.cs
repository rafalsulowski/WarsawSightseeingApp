using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //nazwa aktywnosci
        public string Coordinates { get; set; } = "0 0"; //wspolrzedne x y (wspomagające Address)
        public string Address { get; set; } = string.Empty; //dokladny adress np. Plac Defilad 1
        public string Description { get; set; } = string.Empty; //opis miejsca
        public int EstimatedCost { get; set; } // oszacowane wydatki na miejscu
        public int AverageTimeSpent { get; set; } = 60; //przewidywany potrzebny czas w minutach
        public PlaceAvailabilityTime? PlaceAvailabilityTime { get; set; }   //godzina otwarcia i zamkniecia
        public List<PlacesTrips> TripsThatIncludeThisPlace { get; set; } = new List<PlacesTrips>(); //wycieczki które zawierają na liście miesjc to miejsce
        public List<CommentPlace> Comments { get; set; } = new List<CommentPlace>(); //lista komentarzy do miejsca
        public List<PlaceLikes> Likes { get; set; } = new List<PlaceLikes>();

        public PlaceDTO MapToDTO()
        {
            return new PlaceDTO
            {
                Id = Id,
                Name = Name,
                Coordinates = Coordinates,
                Description = Description,
                EstimatedCost = EstimatedCost,
                Address = Address,
                PlaceAvailabilityTime = PlaceAvailabilityTime != null ? PlaceAvailabilityTime.MapToDTO() : new PlaceAvailabilityTimeDTO(), 
                AverageTimeSpent = AverageTimeSpent,
                TripsThatIncludeThisPlace = new List<PlacesTrips>(),
                Comments = Comments,
                Likes = Likes
            };
        }

        public PlaceDTO MapToDTOWithSeqAndTimeAndBudget(int time, int seq, int budget)
        {
            return new PlaceDTO
            {
                Id = Id,
                Name = Name,
                Coordinates = Coordinates,
                Description = Description,
                EstimatedCost = EstimatedCost,
                Address = Address,
                PlaceAvailabilityTime = PlaceAvailabilityTime != null ? PlaceAvailabilityTime.MapToDTO() : new PlaceAvailabilityTimeDTO(),
                AverageTimeSpent = AverageTimeSpent,
                TripsThatIncludeThisPlace = new List<PlacesTrips>(),
                Comments = Comments,
                Likes = Likes,
                Sequence = seq,
                TimeForPlace = time,
                BudgetForPlace = budget
            };
        }

    }
}
