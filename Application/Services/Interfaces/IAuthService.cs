using Application.DTOs.Auth;

namespace Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default);
}