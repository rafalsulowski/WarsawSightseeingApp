using Azure;
using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Services.CommentPlaceService;
using ProjektZespolowy.Services.PlaceService;
using ProjektZespolowy.Services.UserService;
using System.Security.Claims;

namespace ProjektZespolowy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : Controller
    {
        private readonly IPlaceService _PlaceService;
        private readonly ICommentPlaceService _CommentPlaceService;

        public PlacesController(IPlaceService placeService, ICommentPlaceService CommentPlaceService)
        {
            _PlaceService = placeService;
            _CommentPlaceService = CommentPlaceService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<List<PlaceDTO>>> Get()
        {
            var response = await _PlaceService.GetPlacesAsync(includeProperties: "PlaceAvailabilityTime");
            var dtosList = response.Data.Select(u => u.MapToDTO()).ToList();
            return Ok(dtosList);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepositoryResponse<PlaceDTO>>> Get(int id)
        {
            var response = await _PlaceService.GetPlaceAsync(u => u.Id == id, includeProperties: "PlaceAvailabilityTime");
            if (response.Success && response.Data != null)
            {
                var placeDto = response.Data.MapToDTO();
                return Ok(placeDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("GetPlaceWithComments/{id}")]
        public async Task<ActionResult<RepositoryResponse<PlaceDTO>>> GetWithComments(int id)
        {
            var response = await _PlaceService.GetPlaceAsync(u => u.Id == id, "PlaceAvailabilityTime,Comments");
            if (response.Success)
            {
                var placeDto = response.Data.MapToDTO();
                return Ok(placeDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("GetPlaceWithCommentsAndLikes/{id}")]
        public async Task<ActionResult<RepositoryResponse<PlaceDTO>>> GetWithCommentsAndLikes(int id)
        {
            var response = await _PlaceService.GetPlaceAsync(u => u.Id == id, "PlaceAvailabilityTime,Comments,Likes");
            if (response.Success)
            {
                var placeDto = response.Data.MapToDTO();
                return Ok(placeDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("GetPlaceWithLikes/{id}")]
        public async Task<ActionResult<RepositoryResponse<Place>>> GetWithLikesUsers(int id)
        {
            var response = await _PlaceService.GetPlaceAsync(u => u.Id == id, "PlaceAvailabilityTime,Likes");
            if (response.Success)
            {
                var placeDto = response.Data.MapToDTO();
                return Ok(placeDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("GetPlaceWithTrips/{id}")]
        public async Task<ActionResult<RepositoryResponse<Place>>> GetWithTrips(int id)
        {
            var response = await _PlaceService.GetPlaceAsync(u => u.Id == id, "PlaceAvailabilityTime,TripsThatIncludeThisPlace");
            return Ok(response.Data);
        }

        [HttpPost("AddPlace")]
        public async Task<ActionResult<RepositoryResponse<Place>>> AddPlace([FromBody] CreatePlaceDTO place)
        {
            var placeResponse = await _PlaceService.GetPlaceAsync(u => u.Name == place.Name);
            if (placeResponse.Data != null)
            {
                return new RepositoryResponse<Place> { Success = false, Message = "Activiti with this name already exists" };
            }

            Place newPlace = new Place
            {
                Name = place.Name,
                Address = place.Address,
                Coordinates = place.Coordinates,
                Description = place.Description,
                PlaceAvailabilityTime = new PlaceAvailabilityTime(),
                AverageTimeSpent = place.AverageTimeSpent,
            };

            var response = await _PlaceService.CreatePlace(newPlace);
            return Ok(response.Data);
        }

        // PUT api/<ValuesController>/
        [HttpPut("{id}")]
        public async Task<ActionResult<RepositoryResponse<Place>>> EditPlace([FromBody] Place place)
        {
            var response = await _PlaceService.UpdatePlace(place);
            return Ok(response.Data);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeletePlace(int id)
        {
            var response = await _PlaceService.DeletePlace(new Models.Place() { Id = id });
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpPost("AddCommentToPlace")]
        public async Task<ActionResult<RepositoryResponse<Comment>>> AddCommentToPlace([FromBody] CreateCommentPlaceDTO newComment)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to comment Place!");
            CommentPlace comment = new CommentPlace { Content = newComment.Content, PlaceId = newComment.PlaceId, UserId = int.Parse(userId), Author = newComment.Author, Note = newComment.Note};
            var response = await _CommentPlaceService.CreateCommentPlace(comment);
            return Ok(response.Data);
        }

        [HttpPost("LikePlace")]
        public async Task<ActionResult> LikePlace([FromBody] LikePlaceDTO like)
        {
             string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             if (userId == null)
                return Forbid("User must be loged in to like Place!");
            var Place = await _PlaceService.GetPlaceAsync(u => u.Id == like.PlaceId);
            if (Place == null)
                return Forbid("There isn't such Place!");

            var response = await _PlaceService.LikePlace(new PlaceLikes
            {
                UserLikeId = int.Parse(userId),
                PlaceLikeId = like.PlaceId,
                IsLike = like.IsLiked
            });
            return Ok(response);
        }

        [HttpPost("LikeComment")]
        public async Task<ActionResult> LikeComment([FromBody] LikeCommentDTO like)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to like Place!");
            //var comment = await _CommentService.GetComment(u => u.Id == like.CommentId);
            //if (comment == null)
            //    return Forbid("There isn't such Place!"); TO DO

            var response = await _CommentPlaceService.LikeComment(new CommentPlaceLikes
            {
                UserLikeId = int.Parse(userId),
                CommentPlaceLikeId = like.CommentId,
                IsLike = like.IsLiked
            });
            return Ok(response);
        }

        [HttpDelete("DeletePlaceLike")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteLikePlace(int PlaceId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _PlaceService.DeletePlaceLike(int.Parse(userId), PlaceId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }



    }
}
