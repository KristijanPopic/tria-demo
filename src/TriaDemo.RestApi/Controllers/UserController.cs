using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.RestApi.Controllers.ApiModels;
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
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Generates a JWT token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="validator"></param>
    /// <param name="tokenGenerator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType<UserLoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserLoginRequest>> Login(
        UserLoginRequest request,
        [FromServices]IValidator<UserLoginRequest> validator,
        [FromServices]TokenGenerator tokenGenerator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var user = await userService.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return UnauthorizedProblem("Login failed", $"User with email {request.Email} does not exist.");
        }
        var passwordHasher = new PasswordHasher<User>();
        
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            return UnauthorizedProblem("Login failed", "Wrong password.");
        }
        
        var token = tokenGenerator.GenerateToken(user);
        return Ok(new UserLoginResponse { AccessToken = token });
    }

    private ObjectResult UnauthorizedProblem(string title, string detail)
    {
        return Problem(
            type:"Unauthorized",
            title: title,
            detail: detail, 
            statusCode: StatusCodes.Status401Unauthorized);
    }
}