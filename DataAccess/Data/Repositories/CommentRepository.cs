using System.Linq.Expressions;
using DataAccess;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Comments.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Comment entity, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Comment> entities, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(Comment entity)
    {
        _context.Comments.Update(entity);
    }

    public void UpdateRange(IEnumerable<Comment> entities)
    {
        _context.Comments.UpdateRange(entities);
    }

    public void Remove(Comment entity)
    {
        _context.Comments.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Comment> entities)
    {
        _context.Comments.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetCommentsForTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.TaskId == taskId)
            .Include(c => c.Author)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}