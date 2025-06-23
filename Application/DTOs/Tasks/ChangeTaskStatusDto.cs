using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Tasks;

public class ChangeTaskStatusDto
{
    [Required(ErrorMessage = "Status is required")]
    public TaskStatus Status { get; set; }
}