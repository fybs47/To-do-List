using System.Linq.Expressions;
using DataAccess;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AppDbContext _context;

    public PasswordResetTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetTokens.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<PasswordResetToken>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetTokens.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PasswordResetToken>> FindAsync(Expression<Func<PasswordResetToken, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetTokens.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PasswordResetToken entity, CancellationToken cancellationToken = default)
    {
        await _context.PasswordResetTokens.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<PasswordResetToken> entities, CancellationToken cancellationToken = default)
    {
        await _context.PasswordResetTokens.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(PasswordResetToken entity)
    {
        _context.PasswordResetTokens.Update(entity);
    }

    public void UpdateRange(IEnumerable<PasswordResetToken> entities)
    {
        _context.PasswordResetTokens.UpdateRange(entities);
    }

    public void Remove(PasswordResetToken entity)
    {
        _context.PasswordResetTokens.Remove(entity);
    }

    public void RemoveRange(IEnumerable<PasswordResetToken> entities)
    {
        _context.PasswordResetTokens.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
    }

    public async Task InvalidateUserTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.PasswordResetTokens
            .Where(t => t.UserId == userId && !t.IsUsed)
            .ToListAsync(cancellationToken);
        
        foreach (var token in tokens)
        {
            token.IsUsed = true;
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}