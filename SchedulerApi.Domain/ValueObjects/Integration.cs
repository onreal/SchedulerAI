namespace SchedulerApi.Domain.ValueObjects;

public sealed class Integration
{
    private static readonly Integration ChatGPT = new Integration("ChatGPT", IntegrationType.Generative, 1);
    private static readonly Integration GoogleCalendar = new Integration("GoogleCalendar", IntegrationType.DataSource, 1);
    private static readonly Integration Twilio = new Integration("Twilio", IntegrationType.Transactional, 2);

    public string Name { get; }
    public IntegrationType Type { get; }
    public int Order { get; }

    private Integration(string name, IntegrationType type, int order)
    {
        Name = name;
        Type = type;
        Order = order;
    }

    public static Integration FromName(string name) => name switch
    {
        "ChatGPT" => ChatGPT,
        "GoogleCalendar" => GoogleCalendar,
        "Twilio" => Twilio,
        _ => throw new ArgumentException("Invalid integration name", nameof(name))
    };

    public static IEnumerable<Integration> FromType(IntegrationType type) =>
        All.Where(i => i.Type == type);

    public static IReadOnlyList<Integration> All => new List<Integration> { ChatGPT, GoogleCalendar, Twilio };
}

public enum IntegrationType
{
    DataSource,
    Generative,
    Transactional
}