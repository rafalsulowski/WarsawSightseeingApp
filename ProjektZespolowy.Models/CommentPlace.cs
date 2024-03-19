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
    public class CommentPlace
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public double Note { get; set; }
        public List<CommentPlaceLikes> Likes { get; set; } = new List<CommentPlaceLikes>(); //lista uzytkownikow ktorzy polubili dany post
    }
}
