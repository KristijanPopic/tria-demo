using TriaDemo.Service.Contracts;

namespace TriaDemo.RestApi.Authentication;

public sealed class AnonymousUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;
    public string Email => null!;

    public bool IsAuthenticated => false;
}