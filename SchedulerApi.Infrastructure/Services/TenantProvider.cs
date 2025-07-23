using SchedulerApi.Application.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Services
{
    public interface ITenantProvider
    {
        Guid? GetCurrentUserId();
        bool GetIsProviderRequired();
    }

    public class TenantProvider : ITenantProvider
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public TenantProvider(ICurrentUserAccessor currentUserAccessor)
        {
            _currentUserAccessor = currentUserAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            var user = _currentUserAccessor.GetUser();
            return user?.Id ?? null;
        }

        public bool GetIsProviderRequired()
        {
            return _currentUserAccessor.IsProviderRequired;
        }
    }
}