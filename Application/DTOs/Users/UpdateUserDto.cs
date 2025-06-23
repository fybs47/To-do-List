using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users;

public class UpdateUserDto
{
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string? LastName { get; set; }

    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string? Password { get; set; }
}