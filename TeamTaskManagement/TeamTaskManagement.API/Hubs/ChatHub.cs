using Microsoft.AspNetCore.SignalR;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderName = user,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            // Save message to database
            await _chatRepository.AddMessageAsync(chatMessage);

            // Broadcast to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", chatMessage.SenderName, chatMessage.Content, chatMessage.Timestamp);
        }
    }
}