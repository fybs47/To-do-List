using Domain.Entities;

public class TaskHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid TaskId { get; set; }
    
    public TaskItem Task { get; set; } = null!;
    
    public string ChangedField { get; set; } = string.Empty;
    
    public string OldValue { get; set; } = string.Empty;
    
    public string NewValue { get; set; } = string.Empty;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    public Guid ChangedByUserId { get; set; }
    
    public User ChangedByUser { get; set; } = null!;
}