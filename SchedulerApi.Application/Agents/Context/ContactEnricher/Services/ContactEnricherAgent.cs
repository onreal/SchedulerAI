using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Integrations.Contracts;

namespace SchedulerApi.Application.Agents.Context.ContactEnricher.Services;

public class ContactEnricherAgent(
    IGenerativeIntegrationServices generativeIntegrationServices,
    IConfiguration configuration) : BaseAgent(
    generativeIntegrationServices,
    configuration,
    3,
    "ContactEnricherAgent",
    "SchedulerApi.Application.Prompts.ChatGPT.ExportUserDetailsPrompt.txt"
)
{
    protected override void SetupOnExecuted()
    {
        OnExecuted = async context =>
        {
            Console.WriteLine($"Agent {AgentName} executed with context data.");
            await Task.CompletedTask;
        };
    }
}