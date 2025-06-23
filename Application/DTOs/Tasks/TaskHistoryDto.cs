namespace Application.DTOs.Tasks;

public class TaskHistoryDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string ChangedField { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public Guid ChangedByUserId { get; set; }
    public string ChangedByUserName { get; set; } = string.Empty;
}