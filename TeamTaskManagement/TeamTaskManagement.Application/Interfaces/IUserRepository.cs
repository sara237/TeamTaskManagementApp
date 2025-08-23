using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}