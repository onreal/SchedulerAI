using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Context.ScheduleParser.DTOs;
using SchedulerApi.Application.Agents.Contracts;
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

    public async Task<List<ReminderDto>> GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    {
        var agentContext = new AgentContext();
        
        foreach (var agent in _agentServices)
        {
            agentContext.Prompt = prompt.Message;
            
            await agent.ExecuteAsync(agentContext, cancellationToken);
        }
    }
    
    // public async Task<List<ReminderDto>> GenerateActionsAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    // {
    //     _generativeIntegrationServices.SetSystemMessages(PromptHelper.GetPromptFromResource(
    //         "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"));
    //
    //     var apiUser = _currentUserAccessor.GetUser();
    //     
    //     var reminders =
    //         await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message,
    //             cancellationToken);
    //     
    //     reminders.ForEach(item =>
    //     {
    //         
    //     });
    //     
    //     return await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message, cancellationToken);
    //     
    // }
    //
    // public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    // {
    //     await _transactionalIntegrationServices.SendMessageAsync("+306948896777", message, cancellationToken);
    // }
    //
    // public async Task PerformMessageActionAsync(string message, CancellationToken cancellationToken = default)
    // {
    //     await _transactionalIntegrationServices.SendMessageAsync("+306948896777", message, cancellationToken);
    // }
    //
    // public async Task PerformActionAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    // {
    //     _generativeIntegrationServices.SetSystemMessages(PromptHelper.GetPromptFromResource(
    //         "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"));
    //     
    //     await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message, cancellationToken);
    //     
    //     
    //     
    //
    //     foreach (var reminderAction  in reminders)
    //     {
    //         if (reminderAction.Names != null)
    //             foreach (var reminderActionName in reminderAction.Names)
    //             {
    //                 var recipient = await _recipientRepository.GetByNameAsync(reminderActionName);
    //                 if (recipient == null)
    //                     await _recipientRepository.AddAsync(Domain.Recipient.Recipient.Create());
    //             }
    //
    //         _recipientRepository.GetByNameAsync()
    //         await SendMessageAsync(reminderAction.Note, cancellationToken);
    //     }
    // }
}