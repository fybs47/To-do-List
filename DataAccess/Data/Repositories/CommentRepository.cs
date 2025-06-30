using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Comments.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Comments.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await context.Comments.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Comment entity, CancellationToken cancellationToken = default)
    {
        await context.Comments.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Comment> entities, CancellationToken cancellationToken = default)
    {
        await context.Comments.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(Comment entity)
    {
        context.Comments.Update(entity);
    }

    public void UpdateRange(IEnumerable<Comment> entities)
    {
        context.Comments.UpdateRange(entities);
    }

    public void Remove(Comment entity)
    {
        context.Comments.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Comment> entities)
    {
        context.Comments.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await context.Comments
            .Where(c => c.TaskId == taskId)
            .Include(c => c.Author)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}