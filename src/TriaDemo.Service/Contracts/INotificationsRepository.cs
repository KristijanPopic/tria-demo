using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface INotificationsRepository
{
    Task<IReadOnlyCollection<UserNotification>> CreateAsync(IEnumerable<UserNotification> userNotifications);
}