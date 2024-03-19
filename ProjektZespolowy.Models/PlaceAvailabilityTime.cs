using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models
{
    public class PlaceAvailabilityTime
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int OpeningMonday { get; set; }
        public int OpeningTuesday { get; set; }
        public int OpeningWednesday { get; set; }
        public int OpeningThursday { get; set; }
        public int OpeningFriday { get; set; }
        public int OpeningSaturday { get; set;}
        public int OpeningSunday { get; set; }
        public int ClosingMonday { get; set; }
        public int ClosingTuesday { get; set; }
        public int ClosingWednesday { get; set; }
        public int ClosingThursday { get; set; }
        public int ClosingFriday { get; set; }
        public int ClosingSaturday { get; set; }
        public int ClosingSunday { get; set; }

        public PlaceAvailabilityTimeDTO MapToDTO()
        {
            return new PlaceAvailabilityTimeDTO
            {
                OpeningFriday = OpeningFriday,
                OpeningThursday = OpeningThursday,
                OpeningMonday = OpeningMonday,
                OpeningSaturday = OpeningSaturday,
                OpeningSunday = OpeningSunday,
                OpeningTuesday = OpeningTuesday,
                OpeningWednesday = OpeningWednesday,
                ClosingFriday = ClosingFriday,
                ClosingMonday = ClosingMonday,
                ClosingSaturday = ClosingSaturday,
                ClosingSunday = ClosingSunday,
                ClosingThursday = ClosingThursday,
                ClosingTuesday = ClosingTuesday,
                ClosingWednesday = ClosingWednesday,
            };
        } 
    }
}
