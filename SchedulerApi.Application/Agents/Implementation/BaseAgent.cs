using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Contracts;
using SchedulerApi.Application.Agents.ScheduleParser.DTOs;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;

namespace SchedulerApi.Application.Agents.Implementation;

public abstract class BaseAgent<TResponse> : IAgentService
{
    protected int AgentOrder { get; set; }
    protected string AgentName { get; set; }
    protected string AgentPrompt { get; set; }
    
    protected readonly IGenerativeIntegrationServices GenerativeIntegrationServices;

    protected BaseAgent(
        IGenerativeIntegrationServices generativeIntegrationServices,
        IConfiguration configuration,
        int agentOrder,
        string agentName,
        string agentPrompt) {
        
        AgentOrder = agentOrder;
        AgentName = agentName;
        AgentPrompt = agentPrompt;

        GenerativeIntegrationServices = generativeIntegrationServices;
        GenerativeIntegrationServices.HandleAsync(null, configuration);
    }
    
    public async Task ExecuteAsync(AgentContext context, CancellationToken cancellationToken = default)
    {
        GenerativeIntegrationServices.SetSystemMessages(
            PromptHelper.GetPromptFromResource(
                AgentPrompt
            )
        );
        
        var result = await GenerativeIntegrationServices
            .GenerateMessageAsync<string>(context.Prompt, cancellationToken);
        
        context.AddResult(AgentName, result);
    }

    public int GetAgentOrder()
    {
        return AgentOrder;
    }

    //public abstract Task<TResponse> GenerateAsync(string prompt, CancellationToken cancellationToken = default);

    public string GetAgentName()
    {
        return AgentName;
    }

    public string GetAgentPrompt()
    {
        return AgentPrompt;
    }
}