namespace Domain.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid AuthorId { get; set; }
    
    public Guid TaskId { get; set; }

    public User Author { get; set; } = null!;

    public TaskItem Task { get; set; } = null!;
}