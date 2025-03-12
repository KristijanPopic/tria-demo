using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore.EntityTypeConfigurations;

internal class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notification");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Message)
            .IsRequired()
            .HasMaxLength(1000);
    }
}