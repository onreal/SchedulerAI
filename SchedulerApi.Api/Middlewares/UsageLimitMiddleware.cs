using System.Net;
using SchedulerApi.Application.Attributes;
using SchedulerApi.Domain.ValueObjects;

namespace SchedulerApi.Middlewares;

public class UsageLimitMiddleware(RequestDelegate next, ILogger<UsageLimitMiddleware> logger)
{
    private readonly ILogger<UsageLimitMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var hasAttribute = endpoint?.Metadata.GetMetadata<RequireApiKeyAttribute>() != null;
        
        if (!hasAttribute)
        {
            await next(context);
            return;
        }
        
        if (!context.Items.TryGetValue("User", out var userObj) || userObj is not Domain.User.Entities.User user)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("User not found or unauthorized.");
            return;
        }
        
        Plan userPlan = user.Plan;
        int usageCount = user.UsageCount;

        if (usageCount >= userPlan.UsageLimit)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync($"Usage limit exceeded for plan {userPlan.Name}.");
            return;
        }

        await next(context);
    }
}