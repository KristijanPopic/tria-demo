using Microsoft.AspNetCore.Mvc;
using TriaDemo.Service.Exceptions;

namespace TriaDemo.RestApi.Extensions;

public static class ExceptionExtensions
{
    public static ProblemDetails ToProblemDetails(this Exception exception)
    {
        return exception switch
        {
            FluentValidation.ValidationException validationException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "Bad Request",
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Extensions = new Dictionary<string, object?> { ["errors"] = validationException.Errors }
            },
            ValueNotUniqueException valueNotUniqueException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = "Conflict",
                Title = "Value is already in use.",
                Detail = valueNotUniqueException.Message
            },
            UnauthorizedException unauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Type = "Forbidden",
                Title = "User is not authorized to perform this operation.",
                Detail = unauthorizedException.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Internal Server Error",
                Title = "An error occurred while processing the request",
                Detail = exception.Message
            }
        };
    }
}