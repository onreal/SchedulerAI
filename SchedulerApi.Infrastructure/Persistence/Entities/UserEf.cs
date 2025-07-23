namespace SchedulerApi.Infrastructure.Persistence.Entities;

public class UserEf
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int Plan { get; set; }
    public string ApiKey { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UsageCount { get; set; }
}