using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore;

public class TriaDemoDbContext(DbContextOptions<TriaDemoDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<Group> Groups { get; init; } = null!;
    public DbSet<UserGroup> UserGroups { get; init; } = null!;
    public DbSet<UserNotification> UserNotifications { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TriaDemoDbContext).Assembly);
    }
}