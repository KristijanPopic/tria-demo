using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IUserNotificationsRepository
{
    Task<IReadOnlyCollection<UserNotification>> CreateAsync(IReadOnlyCollection<UserNotification> userNotifications);
    
    Task<UserNotification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<UserNotification> UpdateAsync(UserNotification userNotification, CancellationToken cancellationToken);
}