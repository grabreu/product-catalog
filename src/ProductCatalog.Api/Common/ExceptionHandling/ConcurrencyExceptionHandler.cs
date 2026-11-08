namespace ProductCatalog.Api.Common.ExceptionHandling;

public class ConcurrencyExceptionHandler(ILogger<ConcurrencyExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DbUpdateConcurrencyException)
        {
            return false;
        }

        logger.LogWarning(exception, "A concurrency conflict occurred.");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Detail = "The product was changed by someone else. Try again."
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        return true;
    }
}
