using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using ProjektZespolowy.Services;
using ProjektZespolowy.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjektZespolowy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IUserService userService, IPasswordHasher<User> passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<RepositoryResponse<List<User>>>> Get()
        {
            var response = await _userService.GetUsersAsync();
            return Ok(response.Data);
        }

        [HttpGet("{id}/Likes/Posts")]
        public async Task<ActionResult<UserDTO<Post>>> GetUserWithLikedPosts(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "LikedPosts.Post");
            if (response.Success)
            {
                var listDto = response.Data.LikedPosts.Select(u => u.Post).ToList();
                var userDto = new UserDTO<Post>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = listDto
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Likes/Trips")]
        public async Task<ActionResult<UserDTO<Trip>>> GetUserWithLikedTrips(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "LikedTrips.Trip");
            if (response.Success)
            {
                var listDto = response.Data.LikedTrips.Select(u => u.Trip).ToList();
                var userDto = new UserDTO<Trip>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = listDto
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Likes/Comments")]
        public async Task<ActionResult<UserDTO<Comment>>> GetUserWithLikedComments(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "LikedComments.Comment");
            if (response.Success)
            {
                var listDto = response.Data.LikedComments.Select(u => u.Comment).ToList();
                var userDto = new UserDTO<Comment>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = listDto
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Likes/CommentsPlace")]
        public async Task<ActionResult<UserDTO<CommentPlace>>> GetUserWithLikedCommentsPlace(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "LikedCommentsPlace.CommentPlace");
            if (response.Success)
            {
                var listDto = response.Data.LikedCommentsPlace.Select(u => u.CommentPlace).ToList();
                var userDto = new UserDTO<CommentPlace>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = listDto
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Likes/Places")]
        public async Task<ActionResult<UserDTO<Place>>> GetUserWithLikedPlaces(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "LikedPlaces.Place");
            if (response.Success)
            {
                var listDto = response.Data.LikedPlaces.Select(u => u.Place).ToList();
                var userDto = new UserDTO<Place>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = listDto
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Places")]
        public async Task<ActionResult<UserDTO<Place>>> GetUserWithCreatedPlaces(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "Places.PlaceAvailabilityTime");
            if (response.Success)
            {
                var userDto = new UserDTO<Place>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }


        [HttpGet("{id}/CommentsPlace")]
        public async Task<ActionResult<UserDTO<CommentPlace>>> GetUserWithCreatedCommentsPlace(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "CommentsPlace");
            if (response.Success)
            {
                var userDto = new UserDTO<CommentPlace>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = response.Data.CommentsPlace
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Posts")]
        public async Task<ActionResult<UserDTO<PostDTO>>> GetUserWithCreatedPost(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "Posts");
            if (response.Success)
            {
                var userDto = new UserDTO<PostDTO>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = response.Data.Posts.Select(u => u.MapToDTO()).ToList()
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Trips")]
        public async Task<ActionResult<UserDTO<TripDTO>>> GetUserWithCreatedTrips(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "Trips.Places");
            if (response.Success)
            {
                var userDto = new UserDTO<TripDTO>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = response.Data.Trips.Select(u => u.MapToDTO()).ToList()
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<UserDTO<Comment>>> GetUserWithCreatedComments(int id) //mozna poprawic
        {
            var response = await _userService.GetUserAsync(u => u.Id == id, "Comments");
            if (response.Success)
            {
                var userDto = new UserDTO<Comment>
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username,
                    Data = response.Data.Comments
                };
                return Ok(userDto);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }


        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepositoryResponse<User>>> Get(int id)
        {
            var response = await _userService.GetUserAsync(u => u.Id == id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        // GET api/<ValuesController>/xyz@wp.pl
        [HttpGet("email")]
        public async Task<ActionResult<RepositoryResponse<User>>> Get(string email)
        {
            var response = await _userService.GetUserAsync(u => u.Email == email);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }


        [HttpGet("{id}/friends")]
        public async Task<ActionResult<RepositoryResponse<User>>> GetFriends(int id)
        {
            return BadRequest("DB NOT UPDATED");
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<RepositoryResponse<User>>> Post([FromBody] RegisterUserDTO user)
        {
            var userResponse = await _userService.GetUserAsync(u => u.Email == user.Email);
            if (userResponse.Data != null)
            {
                return new RepositoryResponse<User> { Success = false, Message = "User with this email already exists" };
            }
            userResponse = await _userService.GetUserAsync(u => u.Username == user.Username);
            if (userResponse.Data != null)
            {
                return new RepositoryResponse<User> { Success = false, Message = "User with this username already exists" };
            }

            User newUser = new User
            {
                Email = user.Email,
                Username = user.Username
            };
            var hashed = _passwordHasher.HashPassword(newUser, user.Password);
            newUser.PasswordHash = hashed;
            var response = await _userService.CreateUser(newUser);
            return Ok(response.Data);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RepositoryResponse<User>>> Put([FromBody] User user)
        {
            var response = await _userService.UpdateUser(user);
            return Ok(response.Data);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RepositoryResponse<bool>>> Delete(int id)
        {
            var response = await _userService.DeleteUser(new Models.User() { Id = id });
            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Data);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginUser)
        {
            var userResponse = await _userService.GetUserAsync(u => u.Email == loginUser.Email);

            if (userResponse.Data == null)
                return Forbid("No user with this email exists");

            var user = userResponse.Data;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
            if (result == PasswordVerificationResult.Failed)
                return Forbid("Invalid password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(AuthenticationSettings.JwtExpiresDays);

            var token = new JwtSecurityToken(AuthenticationSettings.Issuer,
                AuthenticationSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);
        }
        [Authorize]
        [HttpGet("UserDataFromToken")]
        public async Task<ActionResult> UserDataFromToken()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid("User must be logged in!");
            var response = await _userService.GetUserAsync(u => u.Id == int.Parse(userId));
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
