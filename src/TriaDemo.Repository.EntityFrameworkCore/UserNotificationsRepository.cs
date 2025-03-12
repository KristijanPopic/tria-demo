using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore;

internal sealed class UserNotificationsRepository(TriaDemoDbContext dbContext) : IUserNotificationsRepository
{
    private readonly DbSet<UserNotification> _dbSet = dbContext.UserNotifications;
    
    public async Task<IReadOnlyCollection<UserNotification>> CreateAsync(IReadOnlyCollection<UserNotification> userNotifications)
    {
        _dbSet.AddRange(userNotifications);
        await dbContext.SaveChangesAsync();
        return userNotifications;
    }

    public async Task<UserNotification> UpdateAsync(UserNotification userNotification, CancellationToken cancellationToken)
    {
        var entry = _dbSet.Entry(userNotification);
        if (entry.State != EntityState.Modified)
        {
            _dbSet.Update(userNotification);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return userNotification;
    }
}