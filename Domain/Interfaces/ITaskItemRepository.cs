using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskItemRepository : IBaseRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetTasksWithDetailsAsync();
    Task<TaskItem?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetTasksByUserAsync(Guid userId);
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
}