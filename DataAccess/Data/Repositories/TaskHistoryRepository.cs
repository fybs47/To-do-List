using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data.Repositories;

public class TaskHistoryRepository(AppDbContext context) : ITaskHistoryRepository
{
    public async Task AddAsync(TaskHistory entity)
    {
        await context.TaskHistories.AddAsync(entity);
    }

    public void Update(TaskHistory entity)
    {
        context.TaskHistories.Update(entity);
    }

    public void Remove(TaskHistory entity)
    {
        context.TaskHistories.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<TaskHistory> GetByIdAsync(Guid id)
    {
        return (await context.TaskHistories.FindAsync(id))!;
    }

    public async Task<IEnumerable<TaskHistory>> GetAllAsync()
    {
        return await context.TaskHistories.ToListAsync();
    }

    public async Task<IEnumerable<TaskHistory>> FindAsync(Expression<Func<TaskHistory, bool>> predicate)
    {
        return await context.TaskHistories.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TaskHistory>> GetHistoryForTaskAsync(Guid taskId)
    {
        return await context.TaskHistories
            .Where(h => h.TaskId == taskId)
            .Include(h => h.ChangedByUser)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task AddHistoryRecordAsync(Guid taskId, string fieldName, string oldValue, string newValue, Guid userId)
    {
        var history = new TaskHistory
        {
            TaskId = taskId,
            ChangedField = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            ChangedByUserId = userId
        };

        await context.TaskHistories.AddAsync(history);
    }
}