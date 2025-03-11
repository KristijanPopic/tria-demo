using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Users;

public sealed class UpdateUserResponse
{
    public required Guid Id { get; set; }
    
    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public bool IsActive { get; set; }
    
    public required IEnumerable<string> Groups { get; set; }

    public static UpdateUserResponse FromUser(User user)
    {
        return new UpdateUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            Groups = user.Groups.Select(g => g.GroupName)
        };
    }
}