using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Data.Configurations;

public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder.HasKey(h => h.Id);
        
        builder.Property(h => h.ChangedField)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(h => h.OldValue)
            .HasMaxLength(255);
        
        builder.Property(h => h.NewValue)
            .HasMaxLength(255);
        
        builder.Property(h => h.ChangedAt)
            .IsRequired();
        
        builder.HasOne(h => h.Task)
            .WithMany(t => t.TaskHistories)
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(h => h.ChangedByUser)
            .WithMany()     
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(h => h.ChangedAt);
        builder.HasIndex(h => h.TaskId);
    }
}