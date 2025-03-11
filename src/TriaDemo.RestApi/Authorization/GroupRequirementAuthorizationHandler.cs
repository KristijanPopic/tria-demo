using Microsoft.AspNetCore.Authorization;
using TriaDemo.Service;

namespace TriaDemo.RestApi.Authorization;

public class GroupRequirementAuthorizationHandler(CurrentUserService currentUserService) : AuthorizationHandler<GroupRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement)
    {
        var hasGroup = await currentUserService.IsInGroup(requirement.GroupName);
        
        if (hasGroup)
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail(new AuthorizationFailureReason(this, "User must be admin to perform this action."));
    }
}