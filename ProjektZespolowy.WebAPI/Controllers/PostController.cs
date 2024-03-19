using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Services.CommentService;
using ProjektZespolowy.Services.PostService;
using ProjektZespolowy.Services.TripService;
using ProjektZespolowy.Services.UserService;
using System.Security.Claims;

namespace ProjektZespolowy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostService _PostService;
        private readonly IUserService _UserService;
        private readonly ICommentService _CommentService;

        public PostController(IPostService postService, ICommentService commentService, IUserService userService)
        {
            _PostService = postService;
            _CommentService = commentService;
            _UserService = userService;
        }

        [HttpGet("GetAllPosts")]
        public async Task<ActionResult<RepositoryResponse<List<PostDTO>>>> GetAllPosts()
        {
            var response = await _PostService.GetPostsAsync();
            return Ok(response.Data.Select(p => p.MapToDTO()).ToList());
        }


        [HttpGet("GetAllPostsWithCommentsAndLikes")]
        public async Task<ActionResult<RepositoryResponse<List<PostDTO>>>> GetAllPostsWithCommentsAndLikes()
        {
            var response = await _PostService.GetPostsAsync(includeProperties: "Comments,Likes");
            var dtosList = response.Data.Select(u => u.MapToDTO()).ToList();

            return Ok(dtosList);
        }


        [HttpGet("GetPostWithComments/{id}")]
        public async Task<ActionResult<RepositoryResponse<PostDTO>>> GetPostWithComments(int id)
        {
            var response = await _PostService.GetPostAsync(p => p.Id == id, "Comments");
            return Ok(response.Data.MapToDTOWithComments());
        }

        [HttpGet("GetPostWithCommentsAndLikes/{id}")]
        public async Task<ActionResult<RepositoryResponse<PostDTO>>> GetPostWithCommentsAndLikes(int id)
        {
            var response = await _PostService.GetPostAsync(p => p.Id == id, "Comments,Likes");
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //przeniesc niektóre z tych rzeczy do funkcji
            PostDTO post = response.Data.MapToDTO();
            if(userId != null && post.Likes.FirstOrDefault(u => u.UserId == int.Parse(userId)) != null)
            {
                post.IsLikedByCurrentUser = true;
            }

            //var comments = response.Data.Comments.Select(u => u.MapToDTO()).ToList(); //tak na szybko
            //var likes = response.Data.Likes.Select(u => u.MapToDTO()).ToList();
            //post.Likes = likes;
            //post.Comments = comments;

            return Ok(post);
        }

        [HttpGet("GetAllPostsWithCommentsAndCommentLikes")]
        public async Task<ActionResult<RepositoryResponse<PostDTO>>> GetAllPostsWithCommentsAndCommentLikes()
        {
            var response = await _PostService.GetPostsAsync(includeProperties:"Comments.Likes");

            //przeniesc niektóre z tych rzeczy do funkcji
            var dtosList = response.Data.Select(u => u.MapToDTO()).ToList();
            //var comments = response.Data.Comments.Select(u => u.MapToDTO()).ToList(); //tak na szybko
            //var likes = response.Data.Likes.Select(u => u.MapToDTO()).ToList();
            //post.Likes = likes;
            //post.Comments = comments;

            return Ok(dtosList);
        }

        [HttpGet("GetPostWithCommentsAndCommentLikes/{id}")]
        public async Task<ActionResult<RepositoryResponse<PostDTO>>> GetPostWithCommentsAndCommentLikes(int id)
        {
            var response = await _PostService.GetPostAsync(p => p.Id == id, "Comments.Likes");

            //przeniesc niektóre z tych rzeczy do funkcji
            PostDTO post = response.Data.MapToDTO();
            //var comments = response.Data.Comments.Select(u => u.MapToDTO()).ToList(); //tak na szybko
            //var likes = response.Data.Likes.Select(u => u.MapToDTO()).ToList();
            //post.Likes = likes;
            //post.Comments = comments;

            return Ok(post);
        }

        [HttpGet("GetPostWithLikesUsers/{id}")]
        public async Task<ActionResult<RepositoryResponse<Post>>> GetPostWithLikesUsers(int id)
        {
            var response = await _PostService.GetPostAsync(p => p.Id == id, "Likes.User");
            return Ok(response.Data);
        }

        [HttpGet("GetPostsByType/{type}")]
        public async Task<ActionResult<RepositoryResponse<List<PostDTO>>>> GetPostsByType(int type)
        {
            var response = await _PostService.GetPostsAsync(p => (byte)p.Type == type);
            return Ok(response.Data.Select(p => p.MapToDTO()).ToList());
        }

        //[Authorize]
        [HttpPost("AddPost")]
        public async Task<ActionResult<RepositoryResponse<bool>>> AddPost([FromBody] CreatePostDTO newPost)
        {
            //TODO walidacja
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to comment post!");

            var userResponse = _UserService.GetUserAsync(u => u.Id == int.Parse(userId)).Result;
            if (userResponse.Data == null)
                return Forbid("User don't exist!");

            Post post = new Post();
            post.Title = newPost.Title;
            post.Content = newPost.Content;
            post.VotesAgainst = 0;
            post.VotesFor = 0;
            post.Type = newPost.Type;
            post.UserId = int.Parse(userId); //zmienic na wyciaganie z usera
            post.Author = userResponse.Data.Username; //zmienic na wyciaganie z usera

            var response = await _PostService.CreatePost(post);
            return Ok(response.Data);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeletePost(int id)
        {
            //TODO sprawdzanie czy usuwa autor posta i usunięcie powiązanych komentarzy
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to comment post!");

            var post = await _PostService.GetPostAsync(u => u.Id == id, "Comments.Likes,Likes");
            if (post.Data == null)
            {
                return Forbid("there isnt such post");
            }
            if (int.Parse(userId) != post.Data.UserId)
            {
                return Forbid("Only owner can delete post");
            }

            var response = await _PostService.DeletePost(post.Data);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<RepositoryResponse<PostDTO>>> EditPost([FromBody] PostDTO editedPost)
        {
            //TODO sprawdzenie czy edytuje autor posta i zrobienie DTO
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to edit Post!");
            if (int.Parse(userId) != editedPost.UserId)
            {
                return Forbid("Only owner can edit post");
            }
            var post = await _PostService.GetPostAsync(u => u.Id == editedPost.Id);
            if (post.Data == null)
            {
                return Forbid("there isnt such post");
            }
            post.Data.Title = editedPost.Title;
            post.Data.Content = editedPost.Content;
            var response = await _PostService.UpdatePost(post.Data);
            return Ok(response.Data);
        }

        [HttpPost("AddCommentToPost")]
        public async Task<ActionResult<RepositoryResponse<Comment>>> AddCommentToPost([FromBody] CreateCommentDTO newComment) //wyjebac usera z dto
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to comment post!");

            var userResponse = _UserService.GetUserAsync(u => u.Id == int.Parse(userId)).Result;
            if(userResponse.Data == null)
                return Forbid("User don't exist!");

            Comment comment = new Comment { Author = userResponse.Data.Username, Content = newComment.Content, PostId = newComment.PostId, UserId = int.Parse(userId) };
            var response = await _CommentService.CreateComment(comment);
            return Ok(response.Data);
        }

        [HttpPost("LikePost")]
        public async Task<ActionResult> LikePost([FromBody] CreateLikePostDTO like)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to like post!");
            var post = await _PostService.GetPostAsync(u => u.Id == like.PostId);
            if (post.Data == null)
                return Forbid("There isn't such post!");

            var response = await _PostService.LikePost(new PostLikes
                                            {
                                             UserLikeId=int.Parse(userId),
                                             PostLikeId=like.PostId,
                                             IsLike=like.IsLiked
                                            });
            return Ok(response);
        }

        [HttpPost("LikeComment")]
        public async Task<ActionResult> LikeComment([FromBody] LikeCommentDTO like)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to like post!");
            //var comment = await _CommentService.GetComment(u => u.Id == like.CommentId);
            //if (comment == null)
            //    return Forbid("There isn't such post!"); TO DO

            var response = await _CommentService.LikeComment(new CommentLikes
            {
                UserLikeId = int.Parse(userId),
                CommentLikeId = like.CommentId,
                IsLike = like.IsLiked
            });
            return Ok(response);
        }

        [HttpDelete("DeletePostLike")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteLikePost(int postId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to delete like post!");
            var response = await _PostService.DeletePostLike(int.Parse(userId), postId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpDelete("DeleteLikeCommentPost")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteLikeCommentPost(int commentId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to delete like post!");
            var response = await _CommentService.DeleteLikeCommentPost(int.Parse(userId), commentId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpDelete("DeleteCommentPost")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteCommentPost(int commentId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to delete comment post!");


            var comenttoDelete = _CommentService.GetCommentAsync(u => u.Id == commentId, "Likes").Result;
            if (comenttoDelete.Data == null)
                return Forbid("There isn't such comment!");

            var response = await _CommentService.DeleteComment(comenttoDelete.Data);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }


        //TODO usuwanie, edytowanie komentarza, obsługa like do posta i komentarza
    }
}
