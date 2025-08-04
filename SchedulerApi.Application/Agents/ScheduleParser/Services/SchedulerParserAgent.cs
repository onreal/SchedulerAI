using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Agents.ScheduleParser.DTOs;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;

namespace SchedulerApi.Application.Agents.ScheduleParser.Services;

public class SchedulerParserAgent(
    IGenerativeIntegrationServices generativeIntegrationServices, IConfiguration configuration) : BaseAgent<List<ReminderDto>>(
    generativeIntegrationServices,
    configuration,
    3,
    "SchedulerParserAgent",
    "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"
);