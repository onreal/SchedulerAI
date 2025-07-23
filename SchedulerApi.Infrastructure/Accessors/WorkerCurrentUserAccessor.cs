using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.User.Entities;

namespace SchedulerApi.Infrastructure.Accessors;

public class WorkerCurrentUserAccessor : ICurrentUserAccessor
{
    public string ApiKey => "system-worker-api-key";
    public bool IsProviderRequired { get; } = false;
    private User? _user { get; set; }

    public User? GetUser()
    {
        return _user;
    }
    
    public void SetUser(User user)
    {
        _user = user;
    }
}