using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Users;

public sealed class UpdateUserRequest
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public bool IsActive { get; set; }

    public required HashSet<string> Groups { get; set; }

    public User Map(User user)
    {
        user.FirstName = FirstName;
        user.LastName = LastName;
        user.IsActive = IsActive;
        user.Groups = Groups.Select(g => new Group{ GroupName = g }).ToList();
        
        return user;
    }
}