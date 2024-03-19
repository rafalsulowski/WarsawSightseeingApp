using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public List<CommentLikesDTO>? Likes { get; set; }
    }
}
