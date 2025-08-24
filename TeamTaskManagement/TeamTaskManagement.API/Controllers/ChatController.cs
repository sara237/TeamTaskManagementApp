using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Infrastructure.Repositories;

namespace TeamTaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatRepository chatRepository, IUserRepository userRepository, ILogger<ChatController> logger)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string userId , CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                var messages = await _chatRepository.GetAllMessagesAsync(cancellationToken);

                // Admin sees all messages
                if (user.Role != "Admin")
                {
                    messages = messages.Where(m => m.SenderId == user.Id).ToList();
                }
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chat messages");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}