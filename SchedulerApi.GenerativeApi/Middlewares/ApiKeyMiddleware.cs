using System.Net;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Domain.User.Contracts;

namespace SchedulerApi.GenerativeApi.Middlewares;

public class ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
{
    private readonly ILogger<ApiKeyMiddleware> _logger = logger;
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string UserItemKey = "User";

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        var endpoint = context.GetEndpoint();
        var requiresApiKey = endpoint?.Metadata.GetMetadata<RequireApiKeyAttribute>() != null;

        if (!requiresApiKey)
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }
            
        extractedApiKey = (string)extractedApiKey!;
            
        var user = await userRepository.GetByApiKeyAsync(extractedApiKey!);

        if (user == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }
            
        context.Items[UserItemKey] = user;

        await next(context);
    }
}