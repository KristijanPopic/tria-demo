using Microsoft.AspNetCore.Diagnostics;
using TriaDemo.RestApi.Extensions;

namespace TriaDemo.RestApi.Exceptions;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        var problemDetails = exception.ToProblemDetails();
        
        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            }
        );
    }
}