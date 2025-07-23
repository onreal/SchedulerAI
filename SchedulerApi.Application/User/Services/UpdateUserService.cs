using SchedulerApi.Application.Exceptions;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Application.User.Contracts;
using SchedulerApi.Application.User.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.User.Contracts;

namespace SchedulerApi.Application.User.Services;

public class UpdateUserService(
    IUserRepository userRepository,
    ICurrentUserAccessor currentUser,
    IUnitOfWork unitOfWork)
    : IUpdateUserService
{
    public async Task UpdateUserAsync(UpdateUserRequest request)
    {
        var user = currentUser.GetUser();
        if (user is null)
            throw new NotFoundException("User not found.");
        
        user.Update(request.Name, request.Surname, request.Email);

        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}