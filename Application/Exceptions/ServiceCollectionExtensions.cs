using Application.Profiles;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Exceptions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        
        services.AddAutoMapper(typeof(TaskProfile).Assembly);
        
        // Если используете FluentValidation
         services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>(ServiceLifetime.Scoped);
        
        return services;
    }
}