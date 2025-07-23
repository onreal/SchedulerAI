namespace SchedulerApi.Application.Shared;

public record ErrorResponse(string Error, IEnumerable<string>? Details = null);