using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjektZespolowy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;


        public List<CommentPlace> CommentsPlace { get; set; } = new List<CommentPlace>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Trip> Trips { get; set; } = new List<Trip>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<PostLikes> LikedPosts { get; set; } = new List<PostLikes>();
        public List<TripLikes> LikedTrips { get; set; } = new List<TripLikes>();
        public List<PlaceLikes> LikedPlaces { get; set; } = new List<PlaceLikes>();
        public List<CommentLikes> LikedComments { get; set; } = new List<CommentLikes>();
        public List<CommentPlaceLikes> LikedCommentsPlace { get; set; } = new List<CommentPlaceLikes>();
    }
}
