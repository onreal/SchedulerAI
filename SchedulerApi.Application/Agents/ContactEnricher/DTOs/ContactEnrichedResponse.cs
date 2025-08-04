namespace SchedulerApi.Application.Agents.ContactEnricher.DTOs;

public record ContactEnrichedResponse(string Name, string Surname, string Email, string Phone, string Address);