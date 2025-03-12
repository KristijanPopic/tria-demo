using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore.EntityTypeConfigurations;

internal class UserNotificationEntityTypeConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.ToTable("UserNotification");

        builder.HasKey(p => p.Id);
        
        // modeling this as two one-to-many relationships, with a unique index on userId and notificationId
        builder
            .HasOne(p => p.Notification)
            .WithMany()
            .HasForeignKey(p => p.NotificationId)
            .IsRequired();
        
        builder
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .IsRequired();
        
        builder
            .HasIndex(b => new { b.NotificationId, b.UserId })
            .IsUnique();
    }
}