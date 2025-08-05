using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Context.IntentClassifier.DTOs;
using SchedulerApi.Application.Agents.Context.ScheduleParser.DTOs;
using SchedulerApi.Application.Agents.Contracts;
using SchedulerApi.Application.Agents.Enums;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Reminder.Contract;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;

namespace SchedulerApi.Application.Reminder.Services;

public class ReminderService : IReminderService
{
    private readonly IEnumerable<IAgentService> _agentServices;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IRecipientRepository _recipientRepository;

    public ReminderService(
        IConfiguration configuration,
        IEnumerable<IAgentService> agentServices,
        ITransactionalIntegrationServices transactionalIntegrationServices,
        ICurrentUserAccessor currentUserAccessor,
        IScheduleRepository scheduleRepository,
        IRecipientRepository recipientRepository
        )
    {
        _agentServices = agentServices
            .OrderBy(agent => agent.GetAgentOrder())
            .ToList();
        
        _currentUserAccessor = currentUserAccessor;
        _scheduleRepository = scheduleRepository;
        _recipientRepository = recipientRepository;
    }

    public async Task<AgentResponse> GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    {
        var agentContext = new AgentContext();
        
        AgentResponse response = null;
        
        foreach (var agent in _agentServices)
        {
            agentContext.Prompt = prompt.Message;
            
            var generated = await agent.ExecuteAsync(agentContext, cancellationToken);
            
            response = agentContext.GetResult<AgentResponse>(agent.GetAgentName());
            
            if (generated == AgentExecutionResult.Stop)
            {
                break;
            }
        }

        if (response == null)
        {
            response = new AgentResponse("Failed", "Failed to generate response.");
        }

        return response;
    }
}