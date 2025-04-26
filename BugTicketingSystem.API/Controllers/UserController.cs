using BugTicketingSystem.BAL.DTOs.UserDtos;
using BugTicketingSystem.BAL.Services.Users;
using BugTicketingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserAddDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var result = await _userService.RegisterUserAsync(userDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result); 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);

            if (!result.IsSuccess)
                return Unauthorized(result.Errors);

            return Ok(new { token = result.Data });
        }
    }
}
