namespace TriaDemo.Service.Models;

public sealed class Group
{
    public Guid Id { get; set; }

    public required string GroupName { get; set; }
    
    public List<User> Users { get; set; } = [];
}