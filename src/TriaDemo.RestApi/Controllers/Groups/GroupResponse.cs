using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Groups;

public sealed class GroupResponse
{
    public Guid Id { get; set; }

    public required string GroupName { get; set; }
    
    public static GroupResponse From(Group group)
    {
        return new GroupResponse { Id = group.Id, GroupName = group.GroupName };
    }
}