using TeamTaskManagement.Domain.Enums;
using TaskStatus = TeamTaskManagement.Domain.Enums.TaskStatus;

namespace TeamTaskManagement.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public required string AssignedUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}