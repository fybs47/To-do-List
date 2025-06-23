using Domain.Interfaces;

public interface ITaskHistoryRepository : IBaseRepository<TaskHistory>
{
    Task<IEnumerable<TaskHistory>> GetHistoryForTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
}