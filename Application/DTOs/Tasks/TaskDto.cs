using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.DTOs.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public Guid? AssigneeId { get; set; }
    public string AssigneeName { get; set; } = string.Empty;
    public int CommentCount { get; set; }
}