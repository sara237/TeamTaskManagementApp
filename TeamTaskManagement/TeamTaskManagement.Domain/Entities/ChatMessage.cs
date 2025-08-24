namespace TeamTaskManagement.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SenderId { get; set; }
        public required string SenderName { get; set; }
        public string SenderRole { get; set; } = "Member"; // جديد: Member أو Admin
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}