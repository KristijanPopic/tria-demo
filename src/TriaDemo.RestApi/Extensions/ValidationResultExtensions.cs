using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Extensions;

public static class ValidationResultExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "Bad Request",
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = validationResult.Errors
            }
        };
    }
}