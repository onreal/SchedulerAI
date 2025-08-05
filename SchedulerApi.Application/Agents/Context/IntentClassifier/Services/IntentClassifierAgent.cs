using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Integrations.Contracts;

namespace SchedulerApi.Application.Agents.Context.IntentClassifier.Services;

public class IntentClassifierAgent(
    IGenerativeIntegrationServices generativeIntegrationServices,
    IConfiguration configuration)
    : BaseAgent(
        generativeIntegrationServices,
        configuration,
        1,
        "IntentClassifierAgent",
        "SchedulerApi.Application.Prompts.ChatGPT.ClassifierPrompt.txt" 
        );