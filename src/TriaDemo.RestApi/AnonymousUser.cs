using TriaDemo.Common;

namespace TriaDemo.RestApi;

public sealed class AnonymousUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;
    public string Email => null!;

    public bool IsAuthenticated => false;
}