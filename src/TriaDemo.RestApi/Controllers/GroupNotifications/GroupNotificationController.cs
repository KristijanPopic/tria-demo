using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Controllers.GroupNotifications;

[Route("api/groups{groupId:guid}")]
[Authorize]
public class GroupNotificationController : ApiControllerBase
{
    /// <summary>
    /// Creates a notification for a group.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("notifications")]
    [ProducesResponseType<CreateGroupNotificationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult> CreateNotification(CreateGroupNotificationRequest request, CancellationToken cancellationToken)
    {
        
    }
}