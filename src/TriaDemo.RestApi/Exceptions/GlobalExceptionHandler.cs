using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.Service.Exceptions;

namespace TriaDemo.RestApi.Exceptions;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            ValueNotUniqueException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "An error occurred while processing the request",
                    Detail = exception.Message
                }
            }
        );
    }
}