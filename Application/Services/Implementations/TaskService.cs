using Application.DTOs.Shared;
using Application.DTOs.Tasks;
using Application.Services.Interfaces;
namespace Application.Services.Implementations;

public class TaskService : ITaskService
{
    public Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, Guid creatorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TaskDto>> GetTasksAsync(TaskFilterDto filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTaskAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> ChangeTaskStatusAsync(Guid taskId, ChangeTaskStatusDto statusDto, Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> AssignTaskAsync(Guid taskId, Guid assigneeId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskHistoryDto>> GetTaskHistoryAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> ExportTasksToCsvAsync(TaskFilterDto filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}