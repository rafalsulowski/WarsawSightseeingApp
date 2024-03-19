using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int PostId { get; set; }
        public List<CommentLikes> Likes { get; set; } = new List<CommentLikes>(); //lista uzytkownikow ktorzy polubili dany post

        public CommentDTO MapToDTO()
        {
            return new CommentDTO
            {
                Id = Id,
                Content = Content,
                Author = Author,
                AuthorId = UserId,
                LikesCount = Likes != null ? Likes.Count : 0,
                Likes = Likes != null ? Likes.Select(u=>u.MapToDTO()).ToList() : null
            };
        }
    }
}
