using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Models;
using ProjektZespolowy.Services.CommentService;
using ProjektZespolowy.Services.TripService;
using System.Security.Claims;
using ProjektZespolowy.Services.UserService;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjektZespolowy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _TripService;
        private readonly ICommentService _CommentService;
        private readonly IUserService _UserService;

        public TripController(ITripService tripService, ICommentService commentService, IUserService userService)
        {
            _TripService = tripService;
            _CommentService = commentService;
            _UserService = userService;
        }

        [HttpGet("GetAllTrips")]
        public async Task<ActionResult<RepositoryResponse<List<TripDTO>>>> GetAllTrips()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to get Trips!");
            var response = await _TripService.GetTripsAsync();
            var dtosList = response.Data.Select(u => u.MapToDTO()).ToList();
            dtosList = dtosList.Where(u => u.IsPublic || u.UserId == int.Parse(userId)).ToList();
            return Ok(dtosList);
        }


        [HttpGet("GetAllTripsWithPlaces")]
        public async Task<ActionResult<RepositoryResponse<List<TripDTO>>>> GetAllTripsWithPlaces()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to get Trips!");
            
            var response = await _TripService.GetTripsAsync(includeProperties:"Places.Place");
            var dtosList = response.Data.Select(u => u.MapToDTO()).ToList();
            dtosList = dtosList.Where(u => u.IsPublic || u.UserId == int.Parse(userId)).ToList();
            
            return Ok(dtosList);
        }

        [HttpGet("GetTripWithPlaces/{id}")]
        public async Task<ActionResult<RepositoryResponse<TripDTO>>> GetTripWithPlaces(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to get Trip!");
            var response = await _TripService.GetTripAsync(p => p.Id == id, "Places.Place");

            if (response.Success && response.Data != null)
            {
                if (!response.Data.IsPublic && response.Data.UserId != int.Parse(userId))
                {
                    return Forbid("This trip isnt public and you arent owner");
                }
                var tripDto = response.Data.MapToDTO();
                return Ok(tripDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }


        //na potrzeby app. Desktop
        [HttpGet("GetPlacesTripsDtoForTrip/{id}")]
        public async Task<ActionResult<RepositoryResponse<List<PlacesTripsDTO>>>> GetPlacesTripsDtoForTrip(int id)
        {
            var response = await _TripService.GetTripAsync(p => p.Id == id, "Places");

            if (response.Success && response.Data != null)
            {
                var palcesTripsDto = response.Data.Places.Select(u => u.MapToDTO()).ToList();
                return Ok(palcesTripsDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        //[HttpGet("GetTripsWithLikes")] //nie wiem czy my w koncu dajemy lajki do tripow??
        //public async Task<ActionResult<RepositoryResponse<Trip>>> GetTripsWithLikes()
        //{
        //    var response = await _TripService.GetTripsAsync(includeProperties: "Likes");
        //    return Ok(response.Data);
        //}

        //[HttpGet("GetTripWithLikesUsers/{id}")]
        //public async Task<ActionResult<RepositoryResponse<Trip>>> GetTripWithLikesUsers(int id)
        //{
        //    var response = await _TripService.GetTripAsync(p => p.Id == id, "Likes.User");
        //    return Ok(response.Data);
        //}

        [Authorize]
        [HttpPost("AddTrip")]
        public async Task<ActionResult<RepositoryResponse<int>>> AddTrip([FromBody] CreateTripDTO newTrip)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to comment post!");

            var userResponse = _UserService.GetUserAsync(u => u.Id == int.Parse(userId)).Result;
            if (userResponse.Data == null)
                return Forbid("User don't exist!");


            Trip trip = new Trip();
            trip.Title = newTrip.Title;
            trip.Description = newTrip.Description;
            trip.Author = userResponse.Data.Username;
            trip.UserId = int.Parse(userId);
            trip.StartHour = newTrip.StartHour;
            trip.StopHour = newTrip.StopHour;
            trip.Date = newTrip.Date;
            trip.TransportType = newTrip.TransportType;
            trip.Budget = newTrip.Budget;
            trip.IsPublic = newTrip.IsPublic;
            trip.Likes = new List<TripLikes>();
            trip.Places = new List<PlacesTrips>();

            var response = await _TripService.CreateTrip(trip);

            //zwraca id dopiero co utworzonej wycieczki
            //potrzebne do dodawania miejsc do wycieczki od razu po jej utworzeniu
            return Ok(response.Data);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteTrip(int id)
        {
            //TODO sprawdzanie czy usuwa autor Tripa i usunięcie powiązanych komentarzy
            var tripDB = await _TripService.GetTripAsync(u => u.Id == id, "Places");
            if (tripDB.Data != null)
            {
                var response = await _TripService.DeleteTrip(tripDB.Data);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                else
                {
                    return BadRequest(response.Data);
                }
            }
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<RepositoryResponse<Trip>>> EditTrip([FromBody] TripDTO editedTrip)
        {
            //TODO sprawdzenie czy edytuje autor Tripa i zrobienie DTO
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to edit Trip!");
            if(int.Parse(userId) != editedTrip.UserId)
            {
                return Forbid("Only owner can edit trip");
            }
            var response = await _TripService.UpdateTrip(editedTrip);
            return Ok(response.Data);
        }


        [HttpPost("LikeTrip")]
        public async Task<ActionResult> LikeTrip([FromBody] LikeTripDTO like)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to like Trip!");
            var Trip = await _TripService.GetTripAsync(u => u.Id == like.TripId);
            if (Trip == null)
                return Forbid("There isn't such Trip!");

            var response = await _TripService.LikeTrip(new TripLikes
            {
                UserLikeId = int.Parse(userId),
                TripLikeId = like.TripId,
                IsLike = like.IsLiked
            });
            return Ok(response);
        }

        //[HttpPost("LikeComment")]
        //public async Task<ActionResult> LikeComment([FromBody] LikeCommentDTO like)
        //{
        //    string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userId == null)
        //        return Forbid("User must be loged in to like Trip!");
        //    //var comment = await _CommentService.GetComment(u => u.Id == like.CommentId);
        //    //if (comment == null)
        //    //    return Forbid("There isn't such Trip!"); TO DO

        //    var response = await _CommentService.LikeComment(new CommentLikes
        //    {
        //        UserLikeId = int.Parse(userId),
        //        CommentLikeId = like.CommentId,
        //        IsLike = like.IsLiked
        //    });
        //    return Ok(response);
        //}

        [HttpDelete("DeleteTripLike")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeleteLikeTrip(int TripId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to like Trip!");
            var response = await _TripService.DeleteTripLike(int.Parse(userId), TripId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }
        //TODO usuwanie, edytowanie komentarza, obsługa like do Tripa i komentarza

        [HttpPost("AddPlaceToTrip")]
        public async Task<ActionResult<RepositoryResponse<bool>>> AddPlaceToTrip([FromBody] AddPlaceToTripDTO newPlace)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to add place to trip!");
            
            var elem = new PlacesTrips
            {
                PlaceId = newPlace.PlaceId,
                TripId = newPlace.TripId,
                Sequence = newPlace.Sequence,
                TimeForPlace = newPlace.TimeForPlace,
                BudgetForPlace = newPlace.BudgetForPlace,
            };

            var response = await _TripService.AddPlaceToTrip(elem);
            return Ok(response.Data);
        }
        [HttpDelete("DeletePlaceFromTrip")]
        public async Task<ActionResult<RepositoryResponse<bool>>> DeletePlaceFromTrip(int placeId, int tripId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be loged in to delete place from trip!");

            var elem = new PlacesTrips
            {
                PlaceId = placeId,
                TripId = tripId,
            };

            var response = await _TripService.DeletePlaceFromTrip(elem);
            return Ok(response.Data);
        }
    }
}
