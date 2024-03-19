using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.JoinedTables
{
    public class CommentLikes
    {
        public int CommentLikeId { get; set; }
        public int UserLikeId { get; set; }
        public Comment Comment { get; set; }
        public User User { get; set; }
        public bool IsLike { get; set; } //do stwierdzenia czy to like czy dislike

        public CommentLikesDTO MapToDTO()
        {
            return new CommentLikesDTO { IsLike = IsLike, CommentId = CommentLikeId, UserId = UserLikeId };
        }
    }
}
