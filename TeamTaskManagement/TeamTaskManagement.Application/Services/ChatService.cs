using Microsoft.Extensions.Logging;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.Application.Services
{
    public class ChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all chat messages");
            cancellationToken.ThrowIfCancellationRequested();
            return await _chatRepository.GetAllMessagesAsync(cancellationToken);
        }

        public async Task SendMessageAsync(ChatMessage message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            message.Timestamp = DateTime.UtcNow;
            await _chatRepository.AddMessageAsync(message, cancellationToken);
            _logger.LogInformation("Message sent by {SenderId} at {Timestamp}", message.SenderId, message.Timestamp);
        }
    }
}