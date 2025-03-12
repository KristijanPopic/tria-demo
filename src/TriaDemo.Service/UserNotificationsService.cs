using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class UserNotificationsService(
    IUserNotificationsRepository userNotificationsRepository,
    IUserRepository userRepository,
    ICurrentUser currentUser) : IUserNotificationsService
{
    public async Task<IReadOnlyCollection<UserNotification>> CreateAsync(IReadOnlyCollection<UserNotification> notifications, CancellationToken cancellationToken)
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

    public async Task<UserNotification?> GetByIdAsync(Guid userNotificationId, CancellationToken cancellationToken)
    {
        return await userNotificationsRepository.GetByIdAsync(userNotificationId, cancellationToken);
    }

    public async Task<UserNotification> UpdateAsync(UserNotification notification, CancellationToken cancellationToken)
    {
        if (currentUser.UserId != notification.UserId)
        {
            throw new UnauthorizedException("User can not update status of other user's notification.");
        }
        
        return await userNotificationsRepository.UpdateAsync(notification, cancellationToken);
    }
}