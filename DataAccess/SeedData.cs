using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskStatus = Domain.Enums.TaskStatus;

namespace DataAccess;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("SeedData");
        var dbContext = services.GetRequiredService<AppDbContext>();

        try
        {
            logger.LogInformation("Starting database seeding...");

            if (!await dbContext.Users.AnyAsync())
            {
                await SeedUsers(dbContext);
                await SeedTasks(dbContext);
                await SeedComments(dbContext);
                await SeedTaskHistories(dbContext);
                
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Database seeded successfully");
            }
            else
            {
                logger.LogInformation("Database already contains data - seeding skipped");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task SeedUsers(AppDbContext context)
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Username = "john_doe",
                Email = "john@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "John",
                LastName = "Doe",
                Role = Role.User,
                IsActive = true
            },
            new User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                Username = "jane_smith",
                Email = "jane@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "Jane",
                LastName = "Smith",
                Role = Role.User,
                IsActive = true
            },
            new User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"),
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPassword123!"),
                FirstName = "Admin",
                LastName = "System",
                Role = Role.Admin,
                IsActive = true
            }
        };

        await context.Users.AddRangeAsync(users);
    }

    private static async Task SeedTasks(AppDbContext context)
    {
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                Title = "Implement authentication",
                Description = "Implement JWT authentication for the API",
                Status = TaskStatus.InProgress,
                Priority = Priority.High,
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                AssigneeId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8")
            },
            new TaskItem
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb2"),
                Title = "Create task management UI",
                Description = "Develop frontend for task management system",
                Status = TaskStatus.New,
                Priority = Priority.Medium,
                DueDate = DateTime.UtcNow.AddDays(14),
                CreatorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                AssigneeId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            },
            new TaskItem
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3"),
                Title = "Setup database",
                Description = "Configure PostgreSQL database with initial schema",
                Status = TaskStatus.Completed,
                Priority = Priority.High,
                DueDate = DateTime.UtcNow.AddDays(-1),
                CreatorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1")
            }
        };

        await context.Tasks.AddRangeAsync(tasks);
    }

    private static async Task SeedComments(AppDbContext context)
    {
        var comments = new List<Comment>
        {
            new Comment
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa9"),
                Text = "Please add more details to the task description",
                AuthorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                TaskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7")
            },
            new Comment
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb4"),
                Text = "I've completed the database setup",
                AuthorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"),
                TaskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3")
            }
        };

        await context.Comments.AddRangeAsync(comments);
    }

    private static async Task SeedTaskHistories(AppDbContext context)
    {
        var histories = new List<TaskHistory>
        {
            new TaskHistory
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb0"),
                TaskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                ChangedField = "Status",
                OldValue = "New",
                NewValue = "InProgress",
                ChangedByUserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            },
            new TaskHistory
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5"),
                TaskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3"),
                ChangedField = "Status",
                OldValue = "InProgress",
                NewValue = "Completed",
                ChangedByUserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1")
            }
        };

        await context.TaskHistories.AddRangeAsync(histories);
    }
}