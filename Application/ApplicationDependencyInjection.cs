using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using FluentValidation;
namespace Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        RegisterServicesManually(services);
        
        return services;
    }

    private static void RegisterServicesManually(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITaskHistoryService, TaskHistoryService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, MemoryCacheService>();
    }
}
