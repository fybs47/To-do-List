using Domain.Enums;

namespace Application.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public Role Role { get; set; }
    
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}