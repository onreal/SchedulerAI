using System.Globalization;
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
    protected Func<AgentContext, Task>? BeforeExecute { get; set; }
    
    protected readonly IGenerativeIntegrationServices GenerativeIntegrationServices;

    protected virtual void SetupBeforeExecute()
    {
        BeforeExecute = null;
    }

    protected virtual void AddUserPrompt(string prompt)
    {
        GenerativeIntegrationServices.SetSystemMessages(prompt);
    }

    protected virtual void ClearUserPrompt()
    {
        GenerativeIntegrationServices.ClearUserMessages();
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
        
        if (BeforeExecute != null)
            await BeforeExecute.Invoke(context);
        
        var result = await GenerativeIntegrationServices
            .GenerateMessageAsync<AgentResponse>(context.Prompt, cancellationToken);
        
        if (OnExecuted != null)
            await OnExecuted.Invoke(context);
        
        context.AddResult(AgentName, result);

        var type = new CultureInfo(result.Type).ToString().ToLower();
        
        return  type is "failed" or "offtopic" ? AgentExecutionResult.Stop : AgentExecutionResult.Continue;
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