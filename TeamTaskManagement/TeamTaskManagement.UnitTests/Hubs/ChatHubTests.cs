
using Microsoft.AspNetCore.SignalR;
using Moq;
using TeamTaskManagement.API.Hubs;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.UnitTests.Hubs
{
    public class ChatHubTests
    {
        [Fact]
        public async Task SendMessage_ShouldBroadcastWithUserName()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mockChatRepo = new Mock<IChatRepository>();
            mockUserRepo.Setup(r => r.GetUserByIdAsync("user-1", It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new User { Id = "user-1", Name = "Alice" });

            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);

            var hub = new ChatHub(mockChatRepo.Object, mockUserRepo.Object)
            {
                Clients = mockClients.Object
            };
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderName = "Alice",
                Content = "Hello team!",
                Timestamp = DateTime.UtcNow
            };
            await mockChatRepo.Object.AddMessageAsync(chatMessage);
            // Act
            await hub.SendMessage("user-1", "Hello team!");

            // Assert
            mockClientProxy.Verify(
                proxy => proxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => (string)o[0] == "Alice" && (string)o[1] == "Hello team!"),
                    default
                ),
                Times.Once
            );
        }
    }
}
