namespace SchedulerApi.Application.Shared.Contracts;

public interface ICurrentUserAccessor
{
    public string ApiKey { get; }
    public bool IsProviderRequired { get; }
    Domain.User.Entities.User? GetUser();
    public void SetUser(Domain.User.Entities.User user);
}