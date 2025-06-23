using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            ILogger<AuthService> logger,
            IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Registering new user: {Email}", registerDto.Email);
            
            if (await _userRepository.GetByEmailAsync(registerDto.Email, cancellationToken) != null)
            {
                throw new ConflictException("Email already registered");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = Domain.Enums.Role.User
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return _tokenService.GenerateTokens(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Login attempt for: {Email}", loginDto.Email);
            
            var user = await _userRepository.GetByEmailAsync(loginDto.Email, cancellationToken) 
                ?? throw new UnauthorizedException("Invalid credentials");

            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedException("Account is deactivated");
            }

            var tokens = _tokenService.GenerateTokens(user);
            
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiry = tokens.RefreshTokenExpiration;
            
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return tokens;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken) 
                ?? throw new NotFoundException("User not found");

            if (user.RefreshToken != refreshTokenDto.RefreshToken || 
                user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            var newTokens = _tokenService.GenerateTokens(user);
            
            user.RefreshToken = newTokens.RefreshToken;
            user.RefreshTokenExpiry = newTokens.RefreshTokenExpiration;
            
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return newTokens;
        }

        public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken) 
                ?? throw new NotFoundException("Invalid refresh token");

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email, cancellationToken);
            if (user == null || !user.IsActive) return;

            var resetToken = await _tokenService.GeneratePasswordResetTokenAsync(user, cancellationToken);
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
        {
            var userId = await _tokenService.ValidatePasswordResetTokenAsync(resetPasswordDto.Token, cancellationToken);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken) 
                ?? throw new NotFoundException("User not found");

            user.PasswordHash = PasswordHelper.HashPassword(resetPasswordDto.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }
    }
}