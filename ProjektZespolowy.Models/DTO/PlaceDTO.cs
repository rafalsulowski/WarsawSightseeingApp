using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //nazwa aktywnosci
        public string Coordinates { get; set; } = "0 0"; //wspolrzedne x y
        public string Description { get; set; } = string.Empty; //opis miejsca
                                                                // public ICollection<User> Likes { get; set; } = new List<User>(); //lista uzytkownikow ktorzy polubili dana aktywnosc, aktualizowana podczas likania w serduszko na stronie
                                                                // public ICollection<User> Visitors { get; set; } = new List<User>(); //lista osob zadeklarowanych do odwiedzenia miejsca, poprzez dodanie aktywnosci jako przystnek podrozy
                                                                //public ICollection<CommentPlace> Comments { get; set; } = new List<CommentPlace>(); //lista komentarzy
        public int EstimatedCost { get; set; } // oszacowane wydatki na miejscu, typ enum
        public string Address { get; set; } = string.Empty; //dokladny adress np. Plac Defilad 1
                                                            //public double AverageNote { get; set; } //srednia ocena wyliczana z ocen z komentarzy miejsca, aktualizowana podczas dodawania nowego komentarza, potrzebne do filtrowania
        public int AverageTimeSpent { get; set; } = 60; //przewidywany potrzebny czas w minutach

        //Czasy otwarcia i zamkniecia to liczba minut od godziny 00:00 -> np. 16:12 to 16*60+12=972
        public PlaceAvailabilityTimeDTO PlaceAvailabilityTime { get; set; } = new PlaceAvailabilityTimeDTO();   //godzina otwarcia i zamkniecia
        public List<PlacesTrips> TripsThatIncludeThisPlace { get; set; } = new List<PlacesTrips>();
        public List<CommentPlace> Comments { get; set; } = new List<CommentPlace>();
        public List<PlaceLikes> Likes { get; set; } = new List<PlaceLikes>();
        public int Sequence { get; set; }
        public int TimeForPlace { get; set; }
        public int BudgetForPlace { get; set; }
    }
}
