using Microsoft.AspNetCore.Mvc;
using TriaDemo.RestApi.Controllers.ApiModels;

namespace TriaDemo.RestApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="request">Request parameters.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<CreateUserRequest>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        return Task.FromResult((ActionResult<CreateUserResponse>)Ok(new CreateUserResponse()));
    }
}