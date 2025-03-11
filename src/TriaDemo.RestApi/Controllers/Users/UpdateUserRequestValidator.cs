using FluentValidation;

namespace TriaDemo.RestApi.Controllers.Users;

internal sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(g => g.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
        RuleFor(u => u.Groups).NotEmpty();
    }
}