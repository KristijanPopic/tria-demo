namespace TriaDemo.RestApi.Controllers.UserNotifications;

public class UserNotificationResponse
{
    public Guid NotificationId { get; set; }

    public Guid UserId { get; set; }

    public bool IsRead { get; set; }
}