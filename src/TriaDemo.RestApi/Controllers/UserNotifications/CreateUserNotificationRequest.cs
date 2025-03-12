using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

public sealed class CreateUserNotificationRequest
{
    public required string Message { get; set; }

    public required IEnumerable<Guid> UserIds { get; set; }
    
    public IReadOnlyCollection<UserNotification> ToUserNotifications()
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Message = Message,
            DateCreated = DateTime.UtcNow
        };
        
        return UserIds.Select(
            uid => new UserNotification
            {
                Id = Guid.NewGuid(),
                UserId = uid,
                NotificationId = notification.Id,
                Notification = notification
            }
        ).ToArray();
    }
}