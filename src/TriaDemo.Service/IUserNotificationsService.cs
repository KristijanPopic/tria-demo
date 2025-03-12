using TriaDemo.Service.Filtering;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface IUserNotificationsService
{
    Task<IReadOnlyCollection<UserNotification>> CreateAsync(IReadOnlyCollection<UserNotification> notifications, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<UserNotification>> GetAsync(Guid userId, NotificationFilters filters, CancellationToken cancellationToken);
    
    Task<UserNotification?> GetByIdAsync(Guid userNotificationId, CancellationToken cancellationToken);
    
    Task<UserNotification> UpdateAsync(UserNotification notification, CancellationToken cancellationToken);
    
}