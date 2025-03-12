namespace TriaDemo.RestApi.Controllers.GroupNotifications;

public class CreateGroupNotificationResponse
{
    public Guid GroupId { get; set; }

    public required string Message { get; set; }
}