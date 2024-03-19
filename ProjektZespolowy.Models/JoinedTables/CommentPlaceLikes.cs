using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.JoinedTables
{
    public class CommentPlaceLikes
    {
        public int CommentPlaceLikeId { get; set; }
        public int UserLikeId { get; set; }
        public CommentPlace CommentPlace { get; set; } = new CommentPlace();
        public User User { get; set; } = new User();
        public bool IsLike { get; set; } //do stwierdzenia czy to like czy dislike
    }
}
