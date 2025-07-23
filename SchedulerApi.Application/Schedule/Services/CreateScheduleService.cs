using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.DTOs;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Schedule.Services;

public class CreateScheduleService(IScheduleRepository repository, IUnitOfWork unitOfWork) : ICreateScheduleService
{
    public async Task<Domain.Schedule.Schedule> CreateAsync(CreateScheduleRequest request)
    {
        if (request.EventTime < DateTime.UtcNow)
        {
            throw new ArgumentException("Event time cannot be in the past.", nameof(request.EventTime));
        }
        
        var schedule = Domain.Schedule.Schedule.Create(
            request.EventTime,
            request.NotifyBeforeMinutes,
            request.RecipientIds,
            request.IntegrationIds
        );

        schedule = await repository.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();
        
        return schedule;
    }
}