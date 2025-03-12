using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

[Route("api/users{userId:guid}")]
[Authorize]
public class NotificationController : ApiControllerBase
{
    /// <summary>
    /// Creates a notification for a user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("notifications")]
    [ProducesResponseType<CreateUserNotificationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult> CreateNotification(CreateUserNotificationRequest request, CancellationToken cancellationToken)
    {
        
    }
}