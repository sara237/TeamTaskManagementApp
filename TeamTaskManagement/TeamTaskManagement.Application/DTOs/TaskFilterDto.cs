namespace TeamTaskManagement.Application.DTOs
{
    public class TaskFilterDto
    {
        public string? Title { get; set; }
        public TaskStatus? Status { get; set; }
        public required string AssignedUserId { get; set; }
    }
}