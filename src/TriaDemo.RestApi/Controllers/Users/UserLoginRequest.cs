namespace TriaDemo.RestApi.Controllers.Users;

public sealed class UserLoginRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}