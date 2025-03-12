using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.Service;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

[Route("api")]
[Authorize]
public class NotificationController(INotificationsService notificationsService) : ApiControllerBase
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
        
        var createdUserNotifications = await notificationsService.CreateNotificationsAsync(userNotifications);
        
        return CreateUserNotificationResponse.FromUserNotifications(createdUserNotifications);
    }
}