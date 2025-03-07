using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.Entities.Configurations;

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
        
        builder.Property(p => p.Username)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasMany(p => p.Groups)
            .WithMany(p => p.Users);
    }
}