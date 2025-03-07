namespace TriaDemo.Common;

public interface IAuthenticatedUser
{
    public Guid UserId { get; }
    
    public string Username { get; }

    public UserGroup Group { get; }
}