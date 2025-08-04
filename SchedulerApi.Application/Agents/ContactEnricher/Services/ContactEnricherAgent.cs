using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.ContactEnricher.DTOs;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;

namespace SchedulerApi.Application.Agents.ContactEnricher.Services;

public class ContactEnricherAgent(
    IGenerativeIntegrationServices generativeIntegrationServices, IConfiguration configuration) : BaseAgent<ContactEnrichedResponse>(
    generativeIntegrationServices,
    configuration,
    3,
    "ContactEnricherAgent",
    "SchedulerApi.Application.Prompts.ChatGPT.ExportUserDetailsPrompt.txt"
);