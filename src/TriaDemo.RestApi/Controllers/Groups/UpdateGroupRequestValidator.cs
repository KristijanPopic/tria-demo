using FluentValidation;

namespace TriaDemo.RestApi.Controllers.Groups;

internal sealed class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
{
    public UpdateGroupRequestValidator()
    {
        RuleFor(g => g.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(g => g.GroupName).NotEmpty();
    }
}