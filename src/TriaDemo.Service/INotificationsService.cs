using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface INotificationsService
{
    Task<IReadOnlyCollection<UserNotification>> CreateNotificationsAsync(IReadOnlyCollection<UserNotification> notifications, CancellationToken cancellationToken);
    
    Task<UserNotification?> GetUserNotificationByIdAsync(Guid userNotificationId, CancellationToken cancellationToken);
    
    Task<UserNotification> UpdateUserNotificationAsync(UserNotification notification, CancellationToken cancellationToken);
    
}