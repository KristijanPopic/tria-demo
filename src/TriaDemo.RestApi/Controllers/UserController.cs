using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.RestApi.Controllers.ApiModels;
using TriaDemo.RestApi.Extensions;
using TriaDemo.Service;
using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers;

[Route("api/users")]
public class UserController(IUserService userService) : ApiControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<CreateUserRequest>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request, [FromServices]IValidator<CreateUserRequest> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var createdUser = await userService.CreateUserAsync(user, cancellationToken);
        
        return Ok(CreateUserResponse.FromUser(createdUser));
    }
}