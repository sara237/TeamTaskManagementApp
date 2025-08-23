using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace TeamTaskManagement.Application.Services
{
    public class UserService : IUserRepository
    {
        private readonly List<User> _users; // In-memory user list
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
            // Initialize with sample users
            _users = new List<User>
            {
                new User { Id ="1", Name = "Alice" },
                new User { Id ="2",Name = "Bob" },
                new User { Id ="3",Name = "Charlie" }
            };
        }

        public Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all users");
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<IEnumerable<User>>(_users);
        }

        public Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching user by Id: {UserId}", userId);
            cancellationToken.ThrowIfCancellationRequested();
            var user = _users.FirstOrDefault(u => u.Id == userId);
            return Task.FromResult(user);
        }
    }
}