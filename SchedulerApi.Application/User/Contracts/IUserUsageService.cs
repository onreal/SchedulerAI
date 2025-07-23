using SchedulerApi.Application.User.DTOs;

namespace SchedulerApi.Application.User.Contracts;

public interface IUserUsageService
{
    Task<UsageResponse> GetUsageAsync();
}