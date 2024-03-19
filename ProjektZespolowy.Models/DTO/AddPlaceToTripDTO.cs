using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class AddPlaceToTripDTO
    {
        public int PlaceId { get; set; }
        public int TripId { get; set; }
        public int Sequence { get; set; }
        public int TimeForPlace { get; set; }
        public int BudgetForPlace { get; set; }
    }
}
