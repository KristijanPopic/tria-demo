using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class NotificationsService(
    IUserNotificationsRepository userNotificationsRepository,
    IUserRepository userRepository,
    ICurrentUser currentUser) : INotificationsService
{
    public async Task<IReadOnlyCollection<UserNotification>> CreateNotificationsAsync(IReadOnlyCollection<UserNotification> notifications, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetByIdAsync(notifications.Select(u => u.UserId), cancellationToken);
        foreach (var userNotification in notifications)
        {
            var userExists = users.TryGetValue(userNotification.UserId, out var existingUser);
            
            // skipping the sender and those that don't exist
            if (!userExists || existingUser!.Id == currentUser.UserId)
            {
                continue;
            }
            
            userNotification.User = existingUser;
        }
        
        var validNotifications = notifications.Where(u => users.ContainsKey(u.UserId)).ToArray();
        
        return await userNotificationsRepository.CreateAsync(validNotifications);
    }

    public Task<UserNotification?> GetUserNotificationByIdAsync(Guid userNotificationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UserNotification> UpdateUserNotificationAsync(UserNotification notification, CancellationToken cancellationToken)
    {
        if (currentUser.UserId != notification.UserId)
        {
            throw new UnauthorizedException("User can not update status of other user's notification.");
        }
        
        return await userNotificationsRepository.UpdateAsync(notification, cancellationToken);
    }
}