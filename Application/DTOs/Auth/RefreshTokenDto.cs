using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class RefreshTokenDto
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}