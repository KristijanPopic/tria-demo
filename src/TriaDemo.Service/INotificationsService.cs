using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface INotificationsService
{
    Task<IReadOnlyCollection<UserNotification>> CreateNotificationsAsync(IReadOnlyCollection<UserNotification> notifications);
    
}