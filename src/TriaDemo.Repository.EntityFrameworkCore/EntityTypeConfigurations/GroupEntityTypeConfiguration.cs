using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore.EntityTypeConfigurations;

internal class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Group");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.GroupName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(p => p.GroupName)
            .IsUnique();
        
        builder.HasData(SeedData.Groups);

    }
}