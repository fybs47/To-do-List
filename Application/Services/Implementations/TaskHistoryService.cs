using Application.DTOs.Tasks;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class TaskHistoryService : ITaskHistoryService
{
    public Task AddHistoryRecordAsync(Guid taskId, string fieldName, string oldValue, string newValue, Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskHistoryDto>> GetHistoryForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}