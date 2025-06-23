// Domain/Entities/Comment.cs

using Domain.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }  
    
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;
}