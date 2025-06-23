using System.Security.Claims;
using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface ITokenService
{
    AuthResponseDto GenerateTokens(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GeneratePasswordResetToken(User user);
    Guid ValidatePasswordResetToken(string token);
}