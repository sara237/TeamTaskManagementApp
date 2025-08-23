using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.Infrastructure.Persistence
{
    public class TeamTaskManagementDbContext : DbContext
    {
        public TeamTaskManagementDbContext(DbContextOptions<TeamTaskManagementDbContext> options)
            : base(options)
        { }

        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasKey(t => t.Id);
            modelBuilder.Entity<ChatMessage>().HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<ChatMessage>()
                .Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);


            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}