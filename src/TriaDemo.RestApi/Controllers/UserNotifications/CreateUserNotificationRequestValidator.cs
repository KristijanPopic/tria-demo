using FluentValidation;

namespace TriaDemo.RestApi.Controllers.UserNotifications;

public sealed class CreateUserNotificationRequestValidator : AbstractValidator<CreateUserNotificationRequest>
{
    public CreateUserNotificationRequestValidator()
    {
        RuleFor(n => n.Message).NotEmpty();
        RuleFor(n => n.UserIds).NotEmpty();
    }
}