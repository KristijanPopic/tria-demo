using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.Service;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

[Route("api")]
[Authorize]
public class NotificationController(IUserNotificationsService userNotificationsService) : ApiControllerBase
{
    /// <summary>
    /// Creates a notification for a user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("user-notifications")]
    [ProducesResponseType<CreateUserNotificationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserNotificationResponse>> CreateNotification(
        CreateUserNotificationRequest request,
        [FromServices] IValidator<CreateUserNotificationRequest> validator,
        CancellationToken cancellationToken)
    { 
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var userNotifications = request.ToUserNotifications();
        
        var createdUserNotifications = await userNotificationsService.CreateAsync(userNotifications, cancellationToken);
        
        return CreateUserNotificationResponse.FromUserNotifications(createdUserNotifications);
    }

    /// <summary>
    /// Marks the notification as read or unread.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <param name="notificationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("users/{userId:guid}/notifications/{notificationId:guid}/status")]
    [ProducesResponseType<UpdateUserNotificationStatusResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateUserNotificationStatusResponse>> UpdateNotification(
        UpdateUserNotificationStatusRequest request,
        Guid userId,
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var userNotification = await userNotificationsService.GetByIdAsync(notificationId, cancellationToken);
        if (userNotification == null || userNotification.UserId != userId)
        {
            return NotFoundProblem("Notification not found", $"User notification id {notificationId} not found");
        }

        if (userNotification.IsRead == request.IsRead)
        {
            return Ok(new UpdateUserNotificationStatusResponse{ IsRead = userNotification.IsRead, NotificationId = notificationId });
        }
        
        userNotification.IsRead = request.IsRead;
        
        await userNotificationsService.UpdateAsync(userNotification, cancellationToken);
        
        return Ok(new UpdateUserNotificationStatusResponse{ IsRead = userNotification.IsRead, NotificationId = notificationId });
    }
}