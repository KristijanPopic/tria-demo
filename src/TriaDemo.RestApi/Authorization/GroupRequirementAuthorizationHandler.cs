using Microsoft.AspNetCore.Authorization;
using TriaDemo.Service;

namespace TriaDemo.RestApi.Authorization;

public sealed class GroupRequirementAuthorizationHandler(CurrentUserService currentUserService) : AuthorizationHandler<GroupRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement)
    {
        var inGroup = await currentUserService.IsInGroup(requirement.GroupName);
        
        if (inGroup)
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail(new AuthorizationFailureReason(this, "User must be admin to perform this action."));
    }
}