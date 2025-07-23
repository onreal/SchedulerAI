namespace SchedulerApi.Application.User.DTOs;

public record UpdateUserRequest(string? Name, string? Surname, string? Email);