using SchedulerApi.Application.User.DTOs;

namespace SchedulerApi.Application.User.Contracts;

public interface IUpdateUserService
{
    Task UpdateUserAsync(UpdateUserRequest request);
}