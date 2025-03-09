using FluentValidation;

namespace TriaDemo.RestApi.Controllers.ApiModels;

internal sealed class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Password).NotEmpty();
    }
}