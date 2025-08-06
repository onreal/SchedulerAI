using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Integrations.Contracts;

namespace SchedulerApi.Application.Agents.Context.ScheduleParser.Services;

public class SchedulerParserAgent(
    IGenerativeIntegrationServices generativeIntegrationServices,
    IConfiguration configuration) : BaseAgent(
    generativeIntegrationServices,
    configuration,
    3,
    "SchedulerParserAgent",
    "SchedulerApi.Application.Prompts.ChatGPT.ExtendedReminderExtractPrompt.txt"
)
{
    protected override void SetupBeforeExecute()
    {
        OnExecuted = async context =>
        {
            Console.WriteLine($"Agent {AgentName} executed with context data.");
            await Task.CompletedTask;
        };
    }
}