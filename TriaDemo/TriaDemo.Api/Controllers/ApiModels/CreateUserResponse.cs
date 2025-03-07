namespace TriaDemo.Api.Controllers.ApiModels;

public class CreateUserResponse
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}