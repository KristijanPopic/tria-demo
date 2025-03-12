using System.Net.Mime;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
    protected ObjectResult ValidationProblem(ValidationResult validationResult)
    {
        return Problem(
            type:"Bad Request",
            title: "Validation failed",
            detail: "One or more validation errors occurred.", 
            statusCode: StatusCodes.Status400BadRequest,
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = validationResult.Errors
            });
    }
    
    protected ObjectResult UnauthorizedProblem(string title, string detail)
    {
        return Problem(
            type:"Unauthorized",
            title: title,
            detail: detail, 
            statusCode: StatusCodes.Status401Unauthorized);
    }
    
    protected ObjectResult ForbiddenProblem(string detail)
    {
        return Problem(
            type:"Forbidden",
            title: "User is not authorized to perform this action.",
            detail: detail, 
            statusCode: StatusCodes.Status403Forbidden);
    }

    protected ObjectResult NotFoundProblem(string title, string detail)
    {
        return Problem(
            type:"Not found",
            title: title,
            detail: detail, 
            statusCode: StatusCodes.Status404NotFound);
    }

    protected ObjectResult ConflictProblem(string detail)
    {
        return Problem(
            type:"Conflict",
            title: "Value is already in use.",
            detail: detail, 
            statusCode: StatusCodes.Status409Conflict);
    }
}