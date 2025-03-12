using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

public sealed class CreateUserNotificationResponse
{
    public required IEnumerable<UserNotificationResponse> UserNotifications { get; set; }
    
    public required string Message { get; set; }
    
    public static CreateUserNotificationResponse FromUserNotifications(IReadOnlyCollection<UserNotification> notifications)
    {
        var a = notifications.Select(
            n => new UserNotificationResponse
            {
                IsRead = n.IsRead,
                NotificationId = n.NotificationId,
                UserId = n.UserId
            }
        );
        return new CreateUserNotificationResponse
        {
            UserNotifications = a,
            Message = notifications.First().Notification!.Message
        };
    }
}