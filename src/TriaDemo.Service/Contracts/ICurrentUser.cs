namespace TriaDemo.Service.Contracts;

public interface ICurrentUser
{
    Guid UserId { get; }
    
    string Email { get; }

    bool IsAuthenticated { get; }
}