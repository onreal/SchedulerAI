using Newtonsoft.Json.Linq;

namespace SchedulerApi.Application.Agents.Implementation;

public record AgentResponse(string Type, JToken Message);