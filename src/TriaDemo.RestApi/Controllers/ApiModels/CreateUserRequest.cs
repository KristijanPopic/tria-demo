namespace TriaDemo.RestApi.Controllers.ApiModels;

public sealed class CreateUserRequest
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Password { get; init; }
}