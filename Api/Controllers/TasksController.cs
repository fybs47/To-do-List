using Application.DTOs.Shared;
using Application.DTOs.Tasks;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace To_do_List.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(
        CreateTaskDto createTaskDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task creation by user {UserId}", userId);
        var task = await _taskService.CreateTaskAsync(createTaskDto, userId, cancellationToken);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} retrieval by user {UserId}", id, userId);
        var task = await _taskService.GetTaskByIdAsync(id, cancellationToken);
        return Ok(task);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskDto>>> GetTasks(
        [FromQuery] TaskFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Tasks retrieval with filters by user {UserId}", userId);
        var tasks = await _taskService.GetTasksAsync(filterDto, cancellationToken);
        return Ok(tasks);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(
        Guid id,
        UpdateTaskDto updateTaskDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} update by user {UserId}", id, userId);
        var task = await _taskService.UpdateTaskAsync(id, updateTaskDto, userId, cancellationToken);
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} deletion by user {UserId}", id, userId);
        await _taskService.DeleteTaskAsync(id, userId, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TaskDto>> ChangeTaskStatus(
        Guid id,
        ChangeTaskStatusDto statusDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} status change by user {UserId}", id, userId);
        var task = await _taskService.ChangeTaskStatusAsync(id, statusDto, userId, cancellationToken);
        return Ok(task);
    }

    [HttpPatch("{id}/assignee")]
    public async Task<ActionResult<TaskDto>> AssignTask(
        Guid id,
        [FromBody] Guid assigneeId,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} assignment by user {UserId} to {AssigneeId}", 
            id, userId, assigneeId);
        var task = await _taskService.AssignTaskAsync(id, assigneeId, userId, cancellationToken);
        return Ok(task);
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<TaskHistoryDto>>> GetTaskHistory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Task {TaskId} history retrieval by user {UserId}", id, userId);
        var history = await _taskService.GetTaskHistoryAsync(id, cancellationToken);
        return Ok(history);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportTasks(
        [FromQuery] TaskFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Tasks export by user {UserId}", userId);
        var stream = await _taskService.ExportTasksToCsvAsync(filterDto, cancellationToken);
        return File(stream, "text/csv", $"tasks_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
    }
}