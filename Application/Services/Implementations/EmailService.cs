using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public Task SendPasswordResetEmailAsync(string email, string resetToken)
        {
            //Заглушка для отправки email
            return Task.CompletedTask;
        }

        public Task SendEmailConfirmationEmailAsync(string email, string confirmationToken)
        {
            return Task.CompletedTask;
        }

        public Task SendNotificationEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}