namespace TriaDemo.RestApi.Controllers.UserNotifications;

public class CreateUserNotificationResponse
{
    public Guid NotificationId { get; set; }
    
    public required string Message { get; set; }
}