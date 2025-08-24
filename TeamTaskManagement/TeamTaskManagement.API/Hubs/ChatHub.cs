using Microsoft.AspNetCore.SignalR;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        public ChatHub(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task SendMessage(string userId, string message)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = user.Id,
                SenderName = user.Name,
                SenderRole = user?.Role ?? "Member",
                Content = message,
                Timestamp = DateTime.UtcNow
            };


            // Save message to database
            await _chatRepository.AddMessageAsync(chatMessage);

            // Broadcast to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", chatMessage);
        }
    }
}