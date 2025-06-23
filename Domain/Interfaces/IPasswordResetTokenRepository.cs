using Domain.Entities;
using Domain.Interfaces;

public interface IPasswordResetTokenRepository : IBaseRepository<PasswordResetToken>
{
    Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task InvalidateUserTokensAsync(Guid userId, CancellationToken cancellationToken = default);
}