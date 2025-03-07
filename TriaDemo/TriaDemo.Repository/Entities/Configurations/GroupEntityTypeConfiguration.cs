using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.Entities.Configurations;

public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Group");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.GroupName)
            .IsRequired()
            .HasMaxLength(100);
    }
}