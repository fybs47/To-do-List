namespace Application.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public Guid TaskId { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
}