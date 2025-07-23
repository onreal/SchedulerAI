using SchedulerApi.Application.Schedule.DTOs;

namespace SchedulerApi.Application.Schedule.Contracts;

public interface ICreateScheduleService
{
    Task<Domain.Schedule.Schedule> CreateAsync(CreateScheduleRequest request);
}