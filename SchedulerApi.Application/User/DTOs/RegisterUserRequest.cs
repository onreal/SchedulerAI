namespace SchedulerApi.Application.User.DTOs;

public record RegisterUserRequest(string Name, string Surname, string Email, string Plan);
