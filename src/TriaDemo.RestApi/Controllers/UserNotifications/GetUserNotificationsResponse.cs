using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

public sealed class GetUserNotificationsResponse
{
    public Guid Id { get; set; }
    
    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime DateCreated { get; set; }

    public static GetUserNotificationsResponse FromUserNotification(UserNotification userNotification)
    {
        return new()
        {
            Id = userNotification.Id,
            Message = userNotification.Notification!.Message,
            IsRead = userNotification.IsRead,
            DateCreated = userNotification.Notification.DateCreated
        };
    }
}