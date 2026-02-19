using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Exceptions; 
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken) {
        var problem = new ProblemDetails();

        switch (exception) {
            case NotFoundException notFound:
                problem.Status = StatusCodes.Status404NotFound;
                problem.Title = $"{notFound.ResourceType} Not Found";
                problem.Detail = notFound.Message;

                problem.Extensions["resourceType"] = notFound.ResourceType;
                problem.Extensions["resourceId"] = notFound.ResourceIdentifier;

                httpContext.Response.StatusCode = 404;
                break;

            case ValidationException validation:
                problem = new ValidationProblemDetails(validation.Errors) {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error"
                };

                foreach (var error in validation.Errors) {
                    problem.Extensions[error.Key] = error.Value;
                }

                httpContext.Response.StatusCode = 400;
                break;

            default:
                logger.LogError(exception, "Unhandled exception occurred: {ErrorMessage}", exception.Message);

                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "Server Error";
                problem.Detail = "An unexpected error occurred.";
                httpContext.Response.StatusCode = 500;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
