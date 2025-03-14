using TriaDemo.Service.Contracts;

namespace TriaDemo.Service.UnitTests.TestDoubles;

public sealed class CurrentUserStub : ICurrentUser
{
    public Guid UserId { get; init; }
    public required string Email { get; init; }
    public bool IsAuthenticated { get; init; }
}