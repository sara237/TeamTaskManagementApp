using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Application.Services;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.UnitTests.TestHelpers
{
    public class FakeCancellation
    {
        private readonly Mock<ITaskRepository> _repo = new();
        private readonly TaskService _service;

        public FakeCancellation()
        {
            var logger = NullLogger<TaskService>.Instance;
            _service = new TaskService(_repo.Object, logger);
        }
        [Fact]
        public async Task GetAllTasksAsync_ShouldThrow_WhenCancelled()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel(); // immediately cancel

            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<TaskItem>
                    {
                        new TaskItem { Id = Guid.NewGuid(), Title = "Task A", AssignedUserId = "user-1" }
                    });

            //var service = new TaskService(mockRepo.Object, NullLogger<TaskService>.Instance);

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _service.GetAllTasksAsync(cts.Token));
        }
        [Fact]
        public async Task GetAllTasksAsync_ShouldCancel_DuringExecution()
        {
            // Arrange
            var cts = new CancellationTokenSource();

            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                    .Returns(async (CancellationToken ct) =>
                    {
                        // Simulate long work
                        await Task.Delay(2000, ct);
                        return new List<TaskItem>
                        {
                            new TaskItem { Title = "Task A", AssignedUserId = "user-1" }
                        };
                    });
            // Act
            cts.CancelAfter(100); // cancel after 100ms

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _service.GetAllTasksAsync(cts.Token));
        }

    }
}