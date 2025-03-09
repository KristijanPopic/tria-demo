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
}