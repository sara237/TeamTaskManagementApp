using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;
using TeamTaskManagement.Infrastructure.Persistence;

namespace TeamTaskManagement.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly TeamTaskManagementDbContext _context;

        public ChatRepository(TeamTaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.ChatMessages
                .OrderBy(m => m.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task AddMessageAsync(ChatMessage message, CancellationToken cancellationToken = default)
        {
            await _context.ChatMessages.AddAsync(message, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}