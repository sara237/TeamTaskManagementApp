using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Application.Services;

namespace TeamTaskManagement.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo = new();
        private readonly UserService _service;

        public UserServiceTests()
        {
            var logger = NullLogger<UserService>.Instance;
            _service = new UserService(logger);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = "user-1";

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result.Name.Should().Be("John Doe");

            _repo.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {

            // Act
            var result = await _service.GetUserByIdAsync("non-existing-id");

            // Assert
            result.Should().BeNull("because no user with that Id exists");
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(u => u.Name == "Alice");

        }
    }

}
