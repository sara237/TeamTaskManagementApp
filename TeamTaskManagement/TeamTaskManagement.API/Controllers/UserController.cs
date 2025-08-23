using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.Application.Interfaces;

namespace TeamTaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id, cancellationToken);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}