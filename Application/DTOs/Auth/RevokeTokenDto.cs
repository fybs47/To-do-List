namespace Application.DTOs.Auth;

public class RevokeTokenDto
{
    public string RefreshToken { get; set; } = null!;
}