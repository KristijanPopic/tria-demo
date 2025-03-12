using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Filtering;
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

    public async Task<IReadOnlyCollection<UserNotification>> GetAsync(Guid userId, NotificationFilters filters, CancellationToken cancellationToken)
    {
        var query = _dbSet
            .Include(un => un.Notification)
            .Where(un => un.UserId == userId);
        
        if (filters.IsRead.HasValue)
        {
            query = query.Where(un => un.IsRead == filters.IsRead.Value);
        }
        if (filters.StartDate.HasValue)
        {
            query = query.Where(un => un.Notification!.DateCreated >= filters.StartDate.Value);
        }
        if (filters.EndDate.HasValue)
        {
            query = query.Where(un => un.Notification!.DateCreated <= filters.EndDate.Value);
        }

        return await query.ToArrayAsync(cancellationToken);
    }

    public async Task<UserNotification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
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