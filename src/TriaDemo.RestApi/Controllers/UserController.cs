using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.Common;
using TriaDemo.RestApi.Controllers.ApiModels;
using TriaDemo.Service;
using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers;

[Route("api/users")]
[Authorize]
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
    [AllowAnonymous]
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
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
        if (!user.IsActive)
        {
            return UnauthorizedProblem("Login failed", $"User with email {request.Email} is disabled.");
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult UpdateUser([FromServices]ICurrentUser user, CancellationToken cancellationToken = default)
    {
        return Ok(new { userId = user.UserId, email = user.Email });
    }

    /// <summary>
    /// Deletes the user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFoundProblem(title: "User not found", detail: $"User with id {id} not found.");
        }
        
        await userService.DeleteUserAsync(user, cancellationToken);
        return NoContent();
    }
}