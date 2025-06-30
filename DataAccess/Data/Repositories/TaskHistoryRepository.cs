using System.Linq.Expressions;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public class TaskHistoryRepository : ITaskHistoryRepository
{
    private readonly AppDbContext _context;

    public TaskHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskHistories.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<TaskHistory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskHistories.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskHistory>> FindAsync(Expression<Func<TaskHistory, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.TaskHistories.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TaskHistory entity, CancellationToken cancellationToken = default)
    {
        await _context.TaskHistories.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TaskHistory> entities, CancellationToken cancellationToken = default)
    {
        await _context.TaskHistories.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TaskHistory entity)
    {
        _context.TaskHistories.Update(entity);
    }

    public void UpdateRange(IEnumerable<TaskHistory> entities)
    {
        _context.TaskHistories.UpdateRange(entities);
    }

    public void Remove(TaskHistory entity)
    {
        _context.TaskHistories.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TaskHistory> entities)
    {
        _context.TaskHistories.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskHistory>> GetHistoryForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskHistories
            .Where(h => h.TaskId == taskId)
            .Include(h => h.ChangedByUser)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync(cancellationToken);
    }
}