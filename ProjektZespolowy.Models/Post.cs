using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models
{
    public enum PostType
    {    
        /* Typ postu, od tego zależy wyświetlanie, dopisane pod WebApp w razie problemów polecam się kontaktować na grupie*/
        /* 0 - information 1 - discussion 2 - voting*/
        Information,
        Discussion,
        Voting
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        /* Typ postu, od tego zależy wyświetlanie, dopisane pod WebApp w razie problemów polecam się kontaktować na grupie*/
        /* 0 - information 1 - discussion 2 - voting*/
        public PostType Type { get; set; }
        public int VotesFor { get; set; }
        public int VotesAgainst { get; set; }
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>(); //lista komentarzy
        public List<PostLikes> Likes { get; set; } = new List<PostLikes>(); //lista uzytkownikow ktorzy polubili dany post

        public PostDTO MapToDTO()
        {
            return new PostDTO
            {
                Id = Id,
                Title = Title,
                Content = Content,
                Type = Type,
                VotesFor = VotesFor,
                VotesAgainst = VotesAgainst,
                Author = Author,
                UserId = UserId,
                LikesCount = Likes.Count,
                CommentsCount = Comments.Count,
                IsLikedByCurrentUser = false, //TODO
                Comments = Comments.Select(c => c.MapToDTO()).ToList(),
                Likes = Likes.Select(c => c.MapToDTO()).ToList()
            };
        }

        public PostDTO MapToDTOWithComments()
        {
            return new PostDTO
            {
                Id = Id,
                Title = Title,
                Content = Content,
                Type = Type,
                VotesFor = VotesFor,
                VotesAgainst = VotesAgainst,
                Author = Author,
                UserId = UserId,
                LikesCount = Likes.Count,
                CommentsCount = Comments.Count,
                IsLikedByCurrentUser = false, //TODO
                Comments = Comments.Select(c=> c.MapToDTO()).ToList()
            };
        }
    }
}
