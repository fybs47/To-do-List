namespace Domain.Interfaces;

public interface ITaskHistoryRepository : IBaseRepository<TaskHistory>
{
    Task<IEnumerable<TaskHistory>> GetHistoryForTaskAsync(Guid taskId);
    Task AddHistoryRecordAsync(Guid taskId, string fieldName, string oldValue, string newValue, Guid userId);
}