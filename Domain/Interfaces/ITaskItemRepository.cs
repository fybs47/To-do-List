using Domain.Entities;
using Domain.Interfaces;

public interface ITaskItemRepository : IBaseRepository<TaskItem>
{
    Task<TaskItem?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<TaskItem> GetQueryable();
    Task<IEnumerable<TaskItem>> GetTasksByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(CancellationToken cancellationToken = default);
}