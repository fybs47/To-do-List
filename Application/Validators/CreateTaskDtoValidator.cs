using Application.DTOs.Tasks;
using FluentValidation;

namespace Application.Validators;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid priority value");

        RuleFor(x => x.DueDate)
            .Must(BeValidDueDate)
            .When(x => x.DueDate.HasValue)
            .WithMessage("Due date must be in the future");
    }

    private bool BeValidDueDate(DateTime? dueDate)
    {
        return dueDate > DateTime.UtcNow;
    }
}