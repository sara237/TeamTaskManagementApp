using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;
using TeamTaskManagement.Infrastructure.Persistence;

namespace TeamTaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TeamTaskManagementDbContext _context;

        public TaskRepository(TeamTaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.ToListAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}