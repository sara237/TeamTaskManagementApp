using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.Application.Interfaces
{
    public interface IChatRepository
    {
        Task<IEnumerable<ChatMessage>> GetAllMessagesAsync(CancellationToken cancellationToken = default);
        Task AddMessageAsync(ChatMessage message, CancellationToken cancellationToken = default);
    }
}