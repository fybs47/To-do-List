using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Services.Interfaces;
using Application.Settings;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly TimeSpan _resetTokenExpiration = TimeSpan.FromHours(1);

        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            
            ValidateJwtSettings();
        }

        private void ValidateJwtSettings()
        {
            if (string.IsNullOrEmpty(_jwtSettings.Key))
                throw new InvalidOperationException("JWT Key is not configured");
            
            if (string.IsNullOrEmpty(_jwtSettings.Issuer))
                throw new InvalidOperationException("JWT Issuer is not configured");
            
            if (string.IsNullOrEmpty(_jwtSettings.Audience))
                throw new InvalidOperationException("JWT Audience is not configured");
            
            if (_jwtSettings.ExpiryInMinutes <= 0)
                throw new InvalidOperationException("JWT ExpiryInMinutes must be greater than 0");
            
            if (_jwtSettings.RefreshTokenExpiryInDays <= 0)
                throw new InvalidOperationException("JWT RefreshTokenExpiryInDays must be greater than 0");
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
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays),
        
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        private JwtSecurityToken GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
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
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
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
            await _passwordResetTokenRepository.InvalidateUserTokensAsync(user.Id, cancellationToken);
            
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