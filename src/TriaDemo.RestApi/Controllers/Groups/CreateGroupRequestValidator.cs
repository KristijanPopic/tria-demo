using FluentValidation;

namespace TriaDemo.RestApi.Controllers.Groups;

internal sealed class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator()
    {
        RuleFor(g => g.GroupName).NotEmpty();
    }
}