using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}