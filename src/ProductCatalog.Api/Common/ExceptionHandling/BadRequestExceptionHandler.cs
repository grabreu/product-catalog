namespace ProductCatalog.Api.Common.ExceptionHandling;

public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badRequestException)
        {
            return false;
        }

        logger.LogWarning("A malformed request was rejected: {Message}", badRequestException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = badRequestException.StatusCode,
            Detail = badRequestException.Message
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
