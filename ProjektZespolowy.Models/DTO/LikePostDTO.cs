using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class LikePostDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}
