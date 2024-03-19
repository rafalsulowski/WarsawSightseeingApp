using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class PlaceAvailabilityTimeDTO
    {
        public int OpeningMonday { get; set; }
        public int OpeningTuesday { get; set; }
        public int OpeningWednesday { get; set; }
        public int OpeningThursday { get; set; }
        public int OpeningFriday { get; set; }
        public int OpeningSaturday { get; set; }
        public int OpeningSunday { get; set; }
        public int ClosingMonday { get; set; }
        public int ClosingTuesday { get; set; }
        public int ClosingWednesday { get; set; }
        public int ClosingThursday { get; set; }
        public int ClosingFriday { get; set; }
        public int ClosingSaturday { get; set; }
        public int ClosingSunday { get; set; }
    }
}
