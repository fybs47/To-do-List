using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly TimeSpan _resetTokenExpiration = TimeSpan.FromHours(1);
        private readonly TimeSpan _refreshTokenExpiration = TimeSpan.FromDays(7);

        public TokenService(
            IConfiguration configuration,
            IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _configuration = configuration;
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        public AuthResponseDto GenerateTokens(User user)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            
            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                Expiration = accessToken.ValidTo,
                RefreshTokenExpiration = DateTime.UtcNow.Add(_refreshTokenExpiration)
            };
        }

        private JwtSecurityToken GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                signingCredentials: credentials
            );
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user, CancellationToken cancellationToken = default)
        {
            // Инвалидируем предыдущие токены пользователя
            await _passwordResetTokenRepository.InvalidateUserTokensAsync(user.Id, cancellationToken);
            
            // Генерируем новый токен
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            
            var resetToken = new PasswordResetToken
            {
                Token = token,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.Add(_resetTokenExpiration),
                IsUsed = false
            };
            
            await _passwordResetTokenRepository.AddAsync(resetToken, cancellationToken);
            await _passwordResetTokenRepository.SaveChangesAsync(cancellationToken);
            
            return token;
        }

        public async Task<Guid> ValidatePasswordResetTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var resetToken = await _passwordResetTokenRepository.GetByTokenAsync(token, cancellationToken);
            
            if (resetToken == null)
                throw new UnauthorizedException("Invalid token");
            
            if (resetToken.IsUsed)
                throw new UnauthorizedException("Token already used");
            
            if (resetToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedException("Token expired");
            
            resetToken.IsUsed = true;
            _passwordResetTokenRepository.Update(resetToken);
            await _passwordResetTokenRepository.SaveChangesAsync(cancellationToken);
            
            return resetToken.UserId;
        }
    }
}