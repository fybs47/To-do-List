using System.ComponentModel.DataAnnotations;
using Application.Validators;
using Domain.Enums;

namespace Application.DTOs.Tasks;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Priority is required")]
    public Priority Priority { get; set; } = Priority.Medium;
    
    [FutureDate(ErrorMessage = "Due date must be in the future")]
    public DateTime? DueDate { get; set; }
    
    public Guid? AssigneeId { get; set; }
}