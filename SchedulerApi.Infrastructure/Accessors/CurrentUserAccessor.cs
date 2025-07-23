using Microsoft.AspNetCore.Http;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.User.Entities;

namespace SchedulerApi.Infrastructure.Accessors
{
    public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
    {
        private const string UserItemKey = "User";
        public bool IsProviderRequired { get; } = false;

        public string ApiKey
        {
            get
            {
                var user = GetUser();
                return user?.ApiKey ?? string.Empty;
            }
        }

        public User? GetUser()
        {
            if (httpContextAccessor.HttpContext?.Items.TryGetValue(UserItemKey, out var userObj) == true)
            {
                return userObj as User;
            }

            return null;
        }

        public void SetUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}