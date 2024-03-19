using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class CreateTripDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartHour { get; set; } = string.Empty; //godzina rozpoczecia wycieczki
        public string StopHour { get; set; } = string.Empty;  //godzina zakonczenia wycieczki
        public DateTime Date { get; set; } // data wycieczki
        public TransportType TransportType { get; set; } //rodzaj transportu jakim bedzie sie przemieszczac uzytkownik podczas calej wycieczki
        public int Budget { get; set; } //budzet uzytkownika DO USTALENIA JAK MA WYGLADAC
        public bool IsPublic { get; set; } //0 to prywatny, 1 to publiczny
    }
}
