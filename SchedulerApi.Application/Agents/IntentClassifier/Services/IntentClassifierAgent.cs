using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Agents.Implementation;
using SchedulerApi.Application.Agents.IntentClassifier.DTOs;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Prompts.Helpers;

namespace SchedulerApi.Application.Agents.IntentClassifier.Services;

public class IntentClassifierAgent(
    IGenerativeIntegrationServices generativeIntegrationServices, IConfiguration configuration)
    : BaseAgent<ClassifierResponse>(
        generativeIntegrationServices,
        configuration,
        1,
        "IntentClassifierAgent",
        "SchedulerApi.Application.Prompts.ChatGPT.ClassifierPrompt.txt"
    );