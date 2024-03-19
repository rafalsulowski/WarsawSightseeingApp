using ProjektZespolowy.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.JoinedTables
{
    public class PostLikes
    {
        public int PostLikeId { get; set; }
        public int UserLikeId { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
        public bool IsLike { get; set; } //do stwierdzenia czy to like czy dislike

        public LikePostDTO MapToDTO()
        {
            return new LikePostDTO { IsLiked = IsLike, PostId = PostLikeId, UserId = UserLikeId };
        }
    }
}
