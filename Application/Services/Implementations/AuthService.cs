using Application.DTOs.Auth;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class AuthService : IAuthService
{
    public Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}