using FluentValidation;

namespace TriaDemo.RestApi.Controllers.ApiModels;

internal sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
    }
}