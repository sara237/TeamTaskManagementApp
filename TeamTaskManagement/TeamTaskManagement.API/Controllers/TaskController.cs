using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.Application.Interfaces;
using TeamTaskManagement.Domain.Entities;

namespace TeamTaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository taskRepository, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _taskRepository.GetAllAsync(cancellationToken);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
                if (task == null) return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching task {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItem task, CancellationToken cancellationToken)
        {
            try
            {
                await _taskRepository.AddAsync(task, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TaskItem task, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _taskRepository.GetByIdAsync(id, cancellationToken);
                if (existingTask == null) return NotFound();

                // Update properties
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Status = task.Status;
                existingTask.Priority = task.Priority;
                existingTask.AssignedUserId = task.AssignedUserId;
                existingTask.DueDate = task.DueDate;

                await _taskRepository.UpdateAsync(existingTask, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _taskRepository.DeleteAsync(id, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}