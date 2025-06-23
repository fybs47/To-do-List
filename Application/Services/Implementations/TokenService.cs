using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public AuthResponseDto GenerateTokens(User user)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }

        public string GeneratePasswordResetToken(User user)
        {
            throw new NotImplementedException();
        }

        public Guid ValidatePasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
    
    
    