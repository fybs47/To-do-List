using DataAccess.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services, 
        string connectionString,
        bool isDevelopmentEnvironment)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            
            options.EnableSensitiveDataLogging(isDevelopmentEnvironment);
            options.EnableDetailedErrors(isDevelopmentEnvironment);
        });
        
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        
        return services;
    }
}