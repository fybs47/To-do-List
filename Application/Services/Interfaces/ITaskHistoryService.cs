using Application.DTOs.Tasks;

namespace Application.Services.Interfaces;

public interface ITaskHistoryService
{
    Task AddHistoryRecordAsync(Guid taskId, string fieldName, string oldValue, string newValue, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskHistoryDto>> GetHistoryForTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
}