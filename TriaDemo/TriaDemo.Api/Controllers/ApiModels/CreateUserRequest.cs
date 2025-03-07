using System.ComponentModel.DataAnnotations;

namespace TriaDemo.Api.Controllers.ApiModels;

public sealed class CreateUserRequest
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public string? Password { get; set; }
}