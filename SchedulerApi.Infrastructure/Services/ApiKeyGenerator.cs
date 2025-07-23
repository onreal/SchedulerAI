using SchedulerApi.Domain.User.Contracts;

namespace SchedulerApi.Infrastructure.Services;

public class ApiKeyGenerator : IApiKeyGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}