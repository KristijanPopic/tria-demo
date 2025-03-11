namespace TriaDemo.Service.Models;

public sealed class Group
{
    public const string GroupAdmin = "admin";
    public const string GroupRegular = "regular";
    
    public Guid Id { get; set; }

    public required string GroupName { get; set; }
}