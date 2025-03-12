namespace TriaDemo.RestApi.Controllers.UserNotifications;

public class UpdateUserNotificationStatusResponse
{
    public required Guid NotificationId { get; set; }

    public bool IsRead { get; set; }
}