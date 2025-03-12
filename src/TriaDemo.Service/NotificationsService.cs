using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class NotificationsService(
    INotificationsRepository notificationsRepository,
    IUserRepository userRepository,
    ICurrentUser currentUser) : INotificationsService
{
    public async Task<IReadOnlyCollection<UserNotification>> CreateNotificationsAsync(IReadOnlyCollection<UserNotification> notifications)
    {
        var users = await userRepository.GetUsersByIdAsync(notifications.Select(u => u.UserId));
        foreach (var userNotification in notifications)
        {
            var existingUser = users[userNotification.UserId];
            
            // skipping the sender and those that don't exist
            if (existingUser == null || existingUser.Id == currentUser.UserId)
            {
                continue;
            }
            
            userNotification.User = existingUser;
        }
        
        var validNotifications = notifications.Where(u => users.ContainsKey(u.UserId));
        
        return await notificationsRepository.CreateAsync(validNotifications);
    }
    
}