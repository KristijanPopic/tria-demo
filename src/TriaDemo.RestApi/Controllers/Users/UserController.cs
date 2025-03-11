using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.RestApi.Authentication;
using TriaDemo.RestApi.Authorization;
using TriaDemo.Service;
using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Users;

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
        
        var existingUser = await userService.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return ConflictProblem("User with the given email already exists.");
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

        var createdUser = await userService.CreateAsync(user, cancellationToken);
        
        return Ok(CreateUserResponse.FromUser(createdUser));
    }
    
    /// <summary>
    /// Gets the list of registered users.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<GetUserResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<GetUserResponse>>> GetUsers(CancellationToken cancellationToken = default)
    {
        // this would have paging in real-world application
        var users = await userService.GetAsync(cancellationToken);
        return Ok(users.Select(GetUserResponse.FromUser));
    }
    
    /// <summary>
    /// Gets a single user by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserResponse>> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return UserNotFoundProblem(id);
        }
        return Ok(GetUserResponse.FromUser(user));
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
        
        var user = await userService.GetByEmailAsync(request.Email, cancellationToken);
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

    [HttpPut]
    [ProducesResponseType<UpdateUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAdmin]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(UpdateUserRequest request, IValidator<UpdateUserRequest> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var user = await userService.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return UserNotFoundProblem(request.Id);        
        }
        
        request.Map(user);
        
        var updatedUser = await userService.UpdateAsync(user, cancellationToken);
        
        return Ok(UpdateUserResponse.FromUser(updatedUser));
    }

    /// <summary>
    /// Deletes the user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAdmin]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return UserNotFoundProblem(id);
        }
        
        await userService.DeleteAsync(user, cancellationToken);
        return NoContent();
    }
    
    private ObjectResult UserNotFoundProblem(Guid id)
    {
        return NotFoundProblem(title: "User not found", detail: $"User with id {id} not found.");
    }
}