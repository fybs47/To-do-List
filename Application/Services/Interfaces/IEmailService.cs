namespace Application.Services.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetToken);
    Task SendEmailConfirmationEmailAsync(string email, string confirmationToken);
    Task SendNotificationEmailAsync(string email, string subject, string message);
}