using Microsoft.Extensions.Logging;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;
using TaskStatus = TeamTaskManagement.Domain.Enums.TaskStatus;

namespace TeamTaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all tasks");
            cancellationToken.ThrowIfCancellationRequested();
            return await _taskRepository.GetAllAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching tasks for user {UserId}", userId);
            cancellationToken.ThrowIfCancellationRequested();

            var tasks = await _taskRepository.GetAllAsync(cancellationToken);
            return tasks.Where(t => t.AssignedUserId == userId);
        }
        public async Task<TaskItem?> GetTasksByIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching tasks for task {taskId}", taskId);
            cancellationToken.ThrowIfCancellationRequested();
            return await _taskRepository.GetByIdAsync(taskId, cancellationToken);       
        }
        public async Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty.");

            await _taskRepository.AddAsync(task, cancellationToken);
            _logger.LogInformation("Task created: {TaskId}", task.Id);
            return task;
        }

        public async Task UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task, cancellationToken);
            _logger.LogInformation("Task updated: {TaskId}", task.Id);
        }

        public async Task UpdateTaskStatusAsync(Guid taskId, TaskStatus status, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                       ?? throw new KeyNotFoundException("Task not found.");

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task, cancellationToken);
            _logger.LogInformation("Task status updated: {TaskId} -> {Status}", task.Id, status);
        }

        public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _taskRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Task deleted: {TaskId}", id);
        }
    }
}