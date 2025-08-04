using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Contracts;
using SchedulerApi.Application.Agents.Enums;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;

namespace SchedulerApi.Application.Agents.Implementation;

public abstract class BaseAgent : IAgentService
{
    protected int AgentOrder { get; set; }
    protected string AgentName { get; set; }
    protected string AgentPrompt { get; set; }
    protected Func<AgentContext, Task>? OnExecuted { get; set; }
    
    protected readonly IGenerativeIntegrationServices GenerativeIntegrationServices;

    protected virtual void SetupOnExecuted()
    {
        OnExecuted = null;
    }

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
    
    public async Task<AgentExecutionResult> ExecuteAsync(AgentContext context, CancellationToken cancellationToken = default)
    {
        GenerativeIntegrationServices.SetSystemMessages(
            PromptHelper.GetPromptFromResource(AgentPrompt)
        );
        
        var result = await GenerativeIntegrationServices
            .GenerateMessageAsync<AgentResponse>(context.Prompt, cancellationToken);
        
        context.AddResult(AgentName, result.Message);
        
        if (OnExecuted != null)
            await OnExecuted.Invoke(context);
        
        return result.Success ? AgentExecutionResult.Continue : AgentExecutionResult.Stop;
    }

    public int GetAgentOrder()
    {
        return AgentOrder;
    }

    public string GetAgentName()
    {
        return AgentName;
    }

    public string GetAgentPrompt()
    {
        return AgentPrompt;
    }
}