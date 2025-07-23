using SchedulerApi.Application.User.DTOs;

namespace SchedulerApi.Application.User.Contracts;

public interface IRegisterUserService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
}