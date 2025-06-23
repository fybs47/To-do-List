using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string NewPassword { get; set; } = string.Empty;
}
