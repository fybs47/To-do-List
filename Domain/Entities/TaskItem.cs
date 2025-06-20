using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public TaskStatus Status { get; set; } = TaskStatus.New;
    
    public Priority Priority { get; set; } = Priority.Medium;
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid CreatorId { get; set; }

    public User Creator { get; set; } = null!;
    
    public Guid? AssigneeId { get; set; }
    
    public User? Assignee { get; set; }

    public List<Comment> Comments { get; set; } = [];

    public List<TaskHistory> TaskHistories { get; set; } = [];
}