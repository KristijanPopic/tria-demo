using Microsoft.AspNetCore.Authorization;

namespace TriaDemo.RestApi.Authorization;

public sealed class GroupRequirement(string groupName) : IAuthorizationRequirement
{
    public string GroupName { get; } = groupName;
}