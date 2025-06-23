using System.Security.Claims;
using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface ITokenService
{
    AuthResponseDto GenerateTokens(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task<string> GeneratePasswordResetTokenAsync(User user, CancellationToken cancellationToken = default);
    Task<Guid> ValidatePasswordResetTokenAsync(string token, CancellationToken cancellationToken = default);
}