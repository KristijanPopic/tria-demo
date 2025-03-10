namespace TriaDemo.Common;

public interface ICurrentUser
{
    public Guid UserId { get; }
    
    public string Email { get; }

    public bool IsAuthenticated { get; }
}