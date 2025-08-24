namespace TeamTaskManagement.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "Member";
    }
}