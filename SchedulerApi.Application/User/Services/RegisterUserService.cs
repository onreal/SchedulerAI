using SchedulerApi.Application.User.Contracts;
using SchedulerApi.Application.User.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.User.Contracts;
using SchedulerApi.Domain.ValueObjects;

namespace SchedulerApi.Application.User.Services;

public class RegisterUserService(
    IUserRepository userRepository,
    IApiKeyGenerator apiKeyGenerator,
    IUnitOfWork unitOfWork)
    : IRegisterUserService
{
    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var apiKey = apiKeyGenerator.Generate();
        
        if (await userRepository.GetByEmailAsync(request.Email) != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new Domain.User.Entities.User(
            name: request.Name,
            surname: request.Surname,
            email: request.Email,
            plan: Plan.FromName(request.Plan),
            apiKey: apiKey
        );

        await userRepository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return new RegisterUserResponse(apiKey);
    }
}