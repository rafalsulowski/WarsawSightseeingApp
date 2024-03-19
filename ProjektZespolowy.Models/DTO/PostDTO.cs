using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public PostType Type { get; set; }
        public int VotesFor { get; set; }
        public int VotesAgainst { get; set; }
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsDislikedByCurrentUser { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public List<LikePostDTO>? Likes { get; set; }
    }
}
