using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.DTOs.Tasks;

public class TaskFilterDto
{
    public TaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
    public Guid? CreatorId { get; set; }
    public Guid? AssigneeId { get; set; }
    public string? SearchTerm { get; set; }
    
    [RegularExpression("createdAt|dueDate|priority", ErrorMessage = "Invalid sort field")]
    public string? SortBy { get; set; } = "createdAt";
    
    public bool SortDescending { get; set; } = false;
    
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;
    
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
}