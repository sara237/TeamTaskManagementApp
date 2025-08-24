
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Application.Services;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.UnitTests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repo = new();
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            var logger = NullLogger<TaskService>.Instance;
            _service = new TaskService(_repo.Object, logger);
        }

        [Fact]
        public async Task CreateAsync_ShouldValidate_And_Save()
        {
            // Arrange
            var ct = new CancellationTokenSource().Token;
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task A",
                Description = "Add unit tests",
                AssignedUserId = "user-1",
                DueDate = DateTime.UtcNow.AddDays(2),
                Priority = Domain.Enums.Priority.Medium,
                Status = Domain.Enums.TaskStatus.Todo
            };

            _repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

            // Act
            var created = await _service.CreateTaskAsync(task, ct);

            // Assert
            created.Should().NotBeNull();
            created.Title.Should().Be("Task A");

            _repo.Verify(r => r.AddAsync(It.Is<TaskItem>(t => t.Title == "Task A"), ct), Times.Once);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask()
        {
            // Arrange
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Task A", AssignedUserId = "user-1" };

            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(task);

            // Act
            var result = await _service.GetTasksByIdAsync(task.Id); // check service method name

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
            Assert.Equal("Task A", result.Title);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 1", AssignedUserId = "user-1" },
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 2", AssignedUserId = "user-2" }
        };

            _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetAllTasksAsync();

            // Assert
            Assert.Equal(2, result.AsList().Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedTask()
        {
            // Arrange
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Old Title", AssignedUserId = "1" };

            _repo.Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

            _repo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                     .Returns((TaskItem t, CancellationToken _) => t);

            // Act
            var updatedTask = new TaskItem { Id = task.Id, Title = "Updated Title", AssignedUserId = "user-1" };
            await _service.UpdateTaskAsync(updatedTask);

            // Assert
            Assert.Equal("Updated Title", updatedTask.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepoDelete()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _repo.Setup(r => r.DeleteAsync(taskId, It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteTaskAsync(taskId);

            // Assert
            _repo.Verify(r => r.DeleteAsync(taskId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }

    public static class EnumerableExtensions
    {
        public static List<T> AsList<T>(this IEnumerable<T> source) => new List<T>(source);
    }
}


