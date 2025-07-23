namespace SchedulerApi.Domain.ValueObjects;

public sealed class Plan
{
    private static readonly Plan Free = new Plan(0, "Free", 30);
    private static readonly Plan Pro = new Plan(1, "Pro", 300);
    private static readonly Plan Enterprise = new Plan(2, "Enterprise", 3000);

    public int Key { get; }
    public string Name { get; }
    public int UsageLimit { get; }

    private Plan(int key, string name, int usageLimit)
    {
        Key = key;
        Name = name;
        UsageLimit = usageLimit;
    }

    public static Plan FromKey(int value)
    {
        return value switch
        {
            0 => Free,
            1 => Pro,
            2 => Enterprise,
            _ => throw new ArgumentException("Invalid plan value", nameof(value))
        };
    }
    
    public static Plan FromName(string name)
    {
        return name switch
        {
            "Free" => Free,
            "Pro" => Pro,
            "Enterprise" => Enterprise,
            _ => throw new ArgumentException("Invalid plan name", nameof(name))
        };
    }
}