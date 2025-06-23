using System.Linq.Expressions;
using DataAccess;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;
public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;

    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> FindAsync(Expression<Func<TaskItem, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TaskItem entity, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TaskItem> entities, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TaskItem entity)
    {
        _context.Tasks.Update(entity);
    }

    public void UpdateRange(IEnumerable<TaskItem> entities)
    {
        _context.Tasks.UpdateRange(entities);
    }

    public void Remove(TaskItem entity)
    {
        _context.Tasks.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TaskItem> entities)
    {
        _context.Tasks.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Comments)
            .Include(t => t.TaskHistories)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public IQueryable<TaskItem> GetQueryable()
    {
        return _context.Tasks.AsQueryable();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Where(t => t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed)
            .ToListAsync(cancellationToken);
    }
}