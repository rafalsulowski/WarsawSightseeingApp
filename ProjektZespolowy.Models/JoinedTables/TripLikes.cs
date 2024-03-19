using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.JoinedTables
{
    public class TripLikes
    {
        public int TripLikeId { get; set; }
        public int UserLikeId { get; set; }
        public Trip Trip { get; set; }
        public User User { get; set; }
        public bool IsLike { get; set; } //do stwierdzenia czy to like czy dislike
    }
}
