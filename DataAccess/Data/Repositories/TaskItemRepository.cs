using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Enums.TaskStatus;

namespace DataAccess.Data.Repositories;

public class TaskItemRepository(AppDbContext context) : ITaskItemRepository
{
    public async Task AddAsync(TaskItem entity)
    {
        await context.Tasks.AddAsync(entity);
    }

    public void Update(TaskItem entity)
    {
        context.Tasks.Update(entity);
    }

    public void Remove(TaskItem entity)
    {
        context.Tasks.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<TaskItem> GetByIdAsync(Guid id)
    {
        return (await context.Tasks.FindAsync(id))!;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await context.Tasks.ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> FindAsync(Expression<Func<TaskItem, bool>> predicate)
    {
        return await context.Tasks.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksWithDetailsAsync()
    {
        return await context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Comments)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Tasks
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .Include(t => t.Comments)
            .Include(t => t.TaskHistories)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(Guid userId)
    {
        return await context.Tasks
            .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
    {
        return await context.Tasks
            .Where(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed)
            .ToListAsync();
    }
}