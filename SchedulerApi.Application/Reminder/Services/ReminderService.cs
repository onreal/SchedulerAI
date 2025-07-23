using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;
using SchedulerApi.Application.Reminder.Contract;
using SchedulerApi.Application.Reminder.DTOs;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;

namespace SchedulerApi.Application.Reminder.Services;

public class ReminderService : IReminderService
{
    private readonly IGenerativeIntegrationServices _generativeIntegrationServices;
    private readonly ITransactionalIntegrationServices _transactionalIntegrationServices;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IRecipientRepository _recipientRepository;

    public ReminderService(IConfiguration configuration, IGenerativeIntegrationServices generativeIntegrationServices, ITransactionalIntegrationServices transactionalIntegrationServices, ICurrentUserAccessor currentUserAccessor, IScheduleRepository scheduleRepository, IRecipientRepository recipientRepository)
    {
        _generativeIntegrationServices = generativeIntegrationServices;
        _generativeIntegrationServices.HandleAsync(null, configuration);
        _transactionalIntegrationServices = transactionalIntegrationServices;
        _currentUserAccessor = currentUserAccessor;
        _scheduleRepository = scheduleRepository;
        _recipientRepository = recipientRepository;
        _transactionalIntegrationServices.HandleAsync(null, configuration);
    }

    public async Task<List<ReminderDto>> GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    {
        _generativeIntegrationServices.SetSystemMessages(PromptHelper.GetPromptFromResource(
            "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"));
        
        return await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message, cancellationToken);
    }
    
    public async Task<List<ReminderDto>> GenerateActionsAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    {
        _generativeIntegrationServices.SetSystemMessages(PromptHelper.GetPromptFromResource(
            "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"));

        var apiUser = _currentUserAccessor.GetUser();
        
        var reminders =
            await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message,
                cancellationToken);
        
        reminders.ForEach(item =>
        {
            
        });
        
        return await _generativeIntegrationServices.GenerateMessageAsync<List<ReminderDto>>(prompt.Message, cancellationToken);
        
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        await _transactionalIntegrationServices.SendMessageAsync("+306948896777", message, cancellationToken);
    }
    
    public async Task PerformMessageActionAsync(string message, CancellationToken cancellationToken = default)
    {
        await _transactionalIntegrationServices.SendMessageAsync("+306948896777", message, cancellationToken);
    }

    public async Task PerformActionAsync(ReminderRequest prompt, CancellationToken cancellationToken = default)
    {
        var reminders = await GenerateAsync(prompt, cancellationToken);

        foreach (var reminderAction in from reminder in reminders from reminderAction in reminder.Actions where reminderAction.ActionType == "sms" select reminderAction)
        {
            await SendMessageAsync(reminderAction.Note, cancellationToken);
        }
    }
}