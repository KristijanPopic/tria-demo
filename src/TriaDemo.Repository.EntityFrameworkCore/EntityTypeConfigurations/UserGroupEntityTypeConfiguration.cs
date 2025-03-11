using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore.EntityTypeConfigurations;

internal class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroup");
        
        builder.HasData(SeedData.UserGroups);
    }
}