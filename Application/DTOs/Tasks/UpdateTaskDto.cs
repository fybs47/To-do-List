using System.ComponentModel.DataAnnotations;
using Application.Validators;
using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.DTOs.Tasks;

public class UpdateTaskDto
{
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public TaskStatus? Status { get; set; }
    
    public Priority? Priority { get; set; }
    
    [FutureDate(ErrorMessage = "Due date must be in the future")]
    public DateTime? DueDate { get; set; }
    
    public Guid? AssigneeId { get; set; }
}