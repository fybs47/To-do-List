// Domain/Entities/User.cs
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public Role Role { get; set; } = Role.User;
        
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        
        public List<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
        public List<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }
}