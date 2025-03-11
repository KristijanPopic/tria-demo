namespace TriaDemo.Service.Models;

public sealed class Group
{
    public const string GroupAdmin = "admin";
    public const string GroupReader = "reader";
    
    public Guid Id { get; set; }

    public required string GroupName { get; set; }
}