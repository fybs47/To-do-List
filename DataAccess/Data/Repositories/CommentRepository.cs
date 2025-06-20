using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task AddAsync(Comment entity)
    {
        await context.Comments.AddAsync(entity);
    }

    public void Update(Comment entity)
    {
        context.Comments.Update(entity);
    }

    public void Remove(Comment entity)
    {
        context.Comments.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Comment> GetByIdAsync(Guid id)
    {
        return (await (context.Comments.FindAsync(id)))!;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        return await context.Comments.ToListAsync();
    }

    public async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
    {
        return await context.Comments.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsForTaskAsync(Guid taskId)
    {
        return await context.Comments
            .Where(c => c.TaskId == taskId)
            .Include(c => c.Author)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}