using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class CommentLikesDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public bool IsLike { get; set; } //do stwierdzenia czy to like czy dislike
    }
}
