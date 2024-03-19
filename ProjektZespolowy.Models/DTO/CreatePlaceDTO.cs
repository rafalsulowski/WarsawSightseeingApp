using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjektZespolowy.Models.Place;

namespace ProjektZespolowy.Models.DTO
{
    public class CreatePlaceDTO
    {
        public int UserId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int AverageTimeSpent { get; set; } = 0;
    }
}
