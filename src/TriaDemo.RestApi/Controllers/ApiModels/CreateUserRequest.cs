using System.ComponentModel.DataAnnotations;

namespace TriaDemo.RestApi.Controllers.ApiModels;

public sealed class CreateUserRequest
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string Password { get; set; }
}