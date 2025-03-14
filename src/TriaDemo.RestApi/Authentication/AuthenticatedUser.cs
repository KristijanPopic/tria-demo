using TriaDemo.Service.Contracts;

namespace TriaDemo.RestApi.Authentication;

public sealed class AuthenticatedUser : ICurrentUser
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    
    public bool IsAuthenticated => true;
}