using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;
using TeamTaskManagement.Infrastructure.Persistence;

namespace TeamTaskManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamTaskManagementDbContext _context;

        public UserRepository(TeamTaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}