using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class TripDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string StartHour { get; set; } = string.Empty; //godzina rozpoczecia wycieczki
        public string StopHour { get; set; } = string.Empty;  //godzina zakonczenia wycieczki
        public DateTime Date { get; set; } // data wycieczki
        public TransportType TransportType { get; set; } //rodzaj transportu jakim bedzie sie przemieszczac uzytkownik podczas calej wycieczki
        public int Budget { get; set; } //budzet uzytkownika DO USTALENIA JAK MA WYGLADAC
        public bool IsPublic { get; set; } //0 to prywatny, 1 to publiczny
        public List<PlaceDTO>? Places { get; set; } = new List<PlaceDTO>();
    }
}
