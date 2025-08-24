using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Domain.Entities;
using TeamTaskManagement.Infrastructure.Persistence;
using TeamTaskManagement.Infrastructure.Repositories;

namespace TeamTaskManagement.UnitTests.TestHelpers
{
    public class TaskRepositoryInMemoryTests
    {
        [Fact]
        public async Task Add_And_Get_ShouldWork_With_InMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TeamTaskManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var ctx = new TeamTaskManagementDbContext(options);
            var repo = new TaskRepository(ctx);
            var ct = CancellationToken.None;

            var item = new TaskItem { Id = Guid.NewGuid(), Title = "Test repo", AssignedUserId = "user-1" };
            await repo.AddAsync(item, ct);

            var fromDb = await repo.GetByIdAsync(item.Id, ct);
            fromDb.Should().NotBeNull();
            fromDb!.Title.Should().Be("Test repo");
        }
    }

}
