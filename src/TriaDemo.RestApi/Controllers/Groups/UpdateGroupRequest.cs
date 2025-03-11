namespace TriaDemo.RestApi.Controllers.Groups;

public sealed class UpdateGroupRequest
{
    public Guid Id { get; set; }

    public required string GroupName { get; set; }
}