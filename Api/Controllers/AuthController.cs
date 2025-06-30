using Application.DTOs.Auth;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace To_do_List.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registration attempt for {Email}", registerDto.Email);
        var response = await _authService.RegisterAsync(registerDto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for {Email}", loginDto.Email);
        var response = await _authService.LoginAsync(loginDto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Token refresh attempt");
        var response = await _authService.RefreshTokenAsync(refreshTokenDto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken(RevokeTokenDto revokeTokenDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Token revocation by user {UserId}", userId);
        await _authService.RevokeTokenAsync(revokeTokenDto.RefreshToken, cancellationToken);
        return NoContent();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Password reset request for {Email}", forgotPasswordDto.Email);
        await _authService.ForgotPasswordAsync(forgotPasswordDto, cancellationToken);
        return Ok(new { Message = "Password reset instructions sent" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Password reset attempt");
        await _authService.ResetPasswordAsync(resetPasswordDto, cancellationToken);
        return Ok(new { Message = "Password reset successfully" });
    }
}