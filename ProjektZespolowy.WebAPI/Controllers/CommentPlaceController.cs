using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Services.CommentPlaceService;
using ProjektZespolowy.Services.PlaceService;
using ProjektZespolowy.Services.UserService;

namespace ProjektZespolowy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentPlaceController : Controller
    {
        private readonly ICommentPlaceService _CommentPlaceService;
        private readonly IUserService _UserService;
        private readonly IPlaceService _PlaceService;

        public CommentPlaceController(ICommentPlaceService CommentPlaceService, IUserService UserService, IPlaceService PlaceService)
        {
            _CommentPlaceService = CommentPlaceService;
            _UserService = UserService;
            _PlaceService = PlaceService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<RepositoryResponse<List<CommentPlace>>>> Get()
        {
            var response = await _CommentPlaceService.GetCommentPlacesAsync();
            return Ok(response.Data);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepositoryResponse<CommentPlace>>> Get(int id)
        {
            var response = await _CommentPlaceService.GetCommentPlaceAsync(u => u.Id == id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("placeId/{placeId}")]
        public async Task<ActionResult<RepositoryResponse<CommentPlace>>> GetByPlaceID(int placeId)
        {
            var response = await _CommentPlaceService.GetCommentPlacesAsync(u => u.PlaceId == placeId);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpPost]
        public async Task<ActionResult<RepositoryResponse<CommentPlace>>> Post([FromBody] CreateCommentPlaceDTO commentPlace)
        {
            var responsePlace = await _PlaceService.GetPlaceAsync(u => u.Id == commentPlace.PlaceId);
            if (responsePlace.Data == null)
            {
                return new RepositoryResponse<CommentPlace> { Success = false, Message = "Activiti with this id don't exists" };
            }

            var responseUser = await _UserService.GetUserAsync(u => u.Id == commentPlace.UserId);
            if (responseUser.Data == null)
            {
                return new RepositoryResponse<CommentPlace> { Success = false, Message = "Activiti with this id don't exists" };
            }

            CommentPlace newCommentPlace = new CommentPlace
            {
                UserId = responseUser.Data.Id,
                PlaceId = responsePlace.Data.Id,
                Content = commentPlace.Content,
                Note = commentPlace.Note
            };

            var response = await _CommentPlaceService.CreateCommentPlace(newCommentPlace);
            return Ok(response.Data);
        }

        // PUT api/<ValuesController>/
        [HttpPut("{id}")]
        public async Task<ActionResult<RepositoryResponse<CommentPlace>>> Put([FromBody] CommentPlace CommentPlace)
        {
            var response = await _CommentPlaceService.UpdateCommentPlace(CommentPlace);
            return Ok(response.Data);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RepositoryResponse<bool>>> Delete(int id)
        {
            var response = await _CommentPlaceService.DeleteCommentPlace(new Models.CommentPlace() { Id = id });
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
