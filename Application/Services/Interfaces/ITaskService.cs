using Application.DTOs.Comments;
using Application.DTOs.Shared;
using Application.DTOs.Tasks;

namespace Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, Guid creatorId, CancellationToken cancellationToken = default);
    Task<TaskDto> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task<PagedResponse<TaskDto>> GetTasksAsync(TaskFilterDto filter, CancellationToken cancellationToken = default);
    Task<TaskDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default);
    Task<TaskDto> ChangeTaskStatusAsync(Guid taskId, ChangeTaskStatusDto statusDto, Guid userId, CancellationToken cancellationToken = default);
    Task<TaskDto> AssignTaskAsync(Guid taskId, Guid assigneeId, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskHistoryDto>> GetTaskHistoryAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task<Stream> ExportTasksToCsvAsync(TaskFilterDto filter, CancellationToken cancellationToken = default);
}