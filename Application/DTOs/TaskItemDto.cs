using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.DTOs;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid CreatorId { get; set; }
    public Guid? AssigneeId { get; set; }
}