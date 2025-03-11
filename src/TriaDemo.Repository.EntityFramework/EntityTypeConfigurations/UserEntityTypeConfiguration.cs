using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFramework.EntityTypeConfigurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasMany(p => p.Groups)
            .WithMany()
            .UsingEntity<UserGroup>(
                r => r.HasOne(p => p.Group).WithMany().HasForeignKey(p => p.GroupId),
                l => l.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId)
            );
        
        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        builder.HasData(SeedData.Users);
    }
}