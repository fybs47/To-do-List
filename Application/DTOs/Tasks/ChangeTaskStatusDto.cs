using System.ComponentModel.DataAnnotations;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.DTOs.Tasks;

public class ChangeTaskStatusDto
{
    [Required(ErrorMessage = "Status is required")]
    public TaskStatus Status { get; set; }
}