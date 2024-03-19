using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models
{
    public enum TransportType //sposob przemieszczania sie podczas wycieczki, potrzebne do zorganizowania przejazdow
    {
        Car,
        Bike,
        Onfoot,
        PublicTransport
    }

    public class Trip
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } //klucz obcy do uzytkownika tworzacego dana wycieczke

        public string StartHour { get; set; } = string.Empty; //godzina rozpoczecia wycieczki
        public string StopHour { get; set; } = string.Empty;  //godzina zakonczenia wycieczki
        public DateTime Date { get; set; } // data wycieczki
        public TransportType TransportType { get; set; } //rodzaj transportu jakim bedzie sie przemieszczac uzytkownik podczas calej wycieczki
        public bool IsPublic { get; set; } = false;//0 to prywatny, 1 to publiczny
        public int Budget { get; set; } //budzet uzytkownika DO USTALENIA JAK MA WYGLADAC
        public List<TripLikes> Likes { get; set; } = new List<TripLikes>(); //lista uzytkownikow ktorzy polubili ta wycieczke
        public List<PlacesTrips>? Places { get; set; } = new List<PlacesTrips>();

        public TripDTO MapToDTO()
        {
            TripDTO dto = new TripDTO();
            dto.StartHour = StartHour;
            dto.StopHour = StopHour;
            dto.Date = Date;
            dto.TransportType = TransportType;
            dto.Budget = Budget;
            dto.Title = Title;
            dto.Description = Description;
            dto.Author = Author;
            dto.Id = Id;
            dto.UserId = UserId;
            dto.IsPublic = IsPublic;
            dto.Places = Places.IsNullOrEmpty() ? new List<PlaceDTO>() : Places.Select(u => u.Place.MapToDTOWithSeqAndTimeAndBudget(u.TimeForPlace, u.Sequence, u.BudgetForPlace)).ToList();

            return dto;
        }
    }
}
