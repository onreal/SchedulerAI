using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using SchedulerApi.Application.Exceptions;
using SchedulerApi.Application.Shared;

namespace SchedulerApi.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception caught");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode code = HttpStatusCode.InternalServerError;
        ErrorResponse errorResponse;

        switch (exception)
        {
            case ValidationException ve:
                code = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse("Validation Failed", new[] { ve.Message });
                break;

            case NotFoundException nf:
                code = HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse(nf.Message);
                break;

            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized;
                errorResponse = new ErrorResponse("Unauthorized access.");
                break;
            
            case InvalidOperationException io:
                code = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse(io.Message);
                break;
            
            case ArgumentNullException ane:
                code = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse("Required parameter is missing.", new[] { ane.Message });
                break;
            
            case KeyNotFoundException knf:
                code = HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse(knf.Message);
                break;
            case ArgumentException ae:
                code = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse("Invalid argument provided.", new[] { ae.Message });
                break;

            default:
                errorResponse = new ErrorResponse("An unexpected error occurred.");
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
