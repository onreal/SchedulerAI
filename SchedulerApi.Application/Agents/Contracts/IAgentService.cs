using SchedulerApi.Application.Agents.Implementation;

namespace SchedulerApi.Application.Agents.Contracts;

public interface IAgentService
{
    int GetAgentOrder();
    
    string GetAgentName();
    
    string GetAgentPrompt();
    
    Task ExecuteAsync(AgentContext context, CancellationToken cancellationToken = default);
}