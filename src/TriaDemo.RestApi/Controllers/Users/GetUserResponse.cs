using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Users;

public sealed class GetUserResponse
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public bool IsActive { get; set; }

    public required IEnumerable<string> Groups { get; set; }
    
    public static GetUserResponse FromUser(User user)
    {
        return new GetUserResponse
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