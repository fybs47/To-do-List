using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Comments;

public class UpdateCommentDto
{
    [Required(ErrorMessage = "Comment text is required")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 1000 characters")]
    public string Text { get; set; } = string.Empty;
}