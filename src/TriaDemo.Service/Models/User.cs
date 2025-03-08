namespace TriaDemo.Service.Models;

public sealed class User
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }

    public string? PasswordHash { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public bool IsActive { get; set; }

    public List<Group> Groups { get; set; } = [];
}