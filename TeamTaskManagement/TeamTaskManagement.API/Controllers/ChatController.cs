using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.Application.Interfaces;

namespace TeamTaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatRepository chatRepository, ILogger<ChatController> logger)
        {
            _chatRepository = chatRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _chatRepository.GetAllMessagesAsync(cancellationToken);
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