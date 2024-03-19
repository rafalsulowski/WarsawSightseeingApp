using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class PlacesTripsDTO
    {
        public int PlaceId { get; set; }
        public int TripId { get; set; }
        public int Sequence { get; set; }
        public int TimeForPlace { get; set; }
        public int BudgetForPlace { get; set; }
    }
}
