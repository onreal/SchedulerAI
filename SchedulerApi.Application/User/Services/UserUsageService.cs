using Microsoft.AspNetCore.Http;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Application.User.Contracts;
using SchedulerApi.Application.User.DTOs;

namespace SchedulerApi.Application.User.Services
{
    public class UserUsageService(ICurrentUserAccessor currentUser) : IUserUsageService
    {
        public Task<UsageResponse> GetUsageAsync()
        {
            var user = currentUser.GetUser();

            if (user == null)
                throw new UnauthorizedAccessException("Invalid user object in context");

            return Task.FromResult(new UsageResponse(user.UsageCount, user.Plan.Name));
        }
    }
}