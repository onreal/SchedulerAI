namespace SchedulerApi.Application.Agents.Implementation;

public class AgentContext
{
    public string Prompt { get; set; } = string.Empty;
    public Dictionary<string, object?> Results { get; } = new();
    
    public void AddResult<T>(string key, T? result) => Results[key] = result;
    
    public T? GetResult<T>(string key) => Results.TryGetValue(key, out var value) ? (T)value! : default;
}