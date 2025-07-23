using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;
using SchedulerApi.Domain.UserIntegration;
using SchedulerApi.Integrations.Generative.ChatGpt.Contracts;
using SchedulerApi.Integrations.Generative.ChatGpt.DTOs;

namespace SchedulerApi.Integrations.Generative.ChatGpt.Services;

public class ChatGptIntegrationService : IChatGptIntegrationService
{
    public string Name => "ChatGPT";

    private OpenAIClient _client;
    private ChatGptSettings _settings;
    private readonly List<ChatMessage> _chatMessages = new();

    public void SetSystemMessages(string  chatMessage)
    {
        _chatMessages.Add(ChatMessage.CreateSystemMessage(chatMessage));
    }
    
    private void SetUserMessages(string  chatMessage)
    {
        _chatMessages.Add(ChatMessage.CreateUserMessage(chatMessage));
    }

    public async Task HandleAsync(UserIntegration userIntegration, IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var settingsJson = userIntegration != null
            ? userIntegration.Settings
            : JsonConvert.SerializeObject(configuration.GetSection("Integrations:ChatGPT").Get<ChatGptSettings>());
        var settings = JsonConvert.DeserializeObject<ChatGptSettings>(settingsJson ?? "{}");

        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "ChatGPT settings cannot be null.");
        }

        _settings = settings;

        _client = new OpenAIClient(_settings.ApiKey);
    }

    public async Task<T> GenerateMessageAsync<T>(string prompt,
        CancellationToken cancellationToken = default)
    {
        SetUserMessages(prompt);

        var chatClient = _client.GetChatClient(_settings.Model);

        var response = await chatClient.CompleteChatAsync(_chatMessages.ToList(), cancellationToken: cancellationToken);
        var cleaned = System.Text.RegularExpressions.Regex.Unescape(response.Value.Content[0].Text);
        return JsonConvert.DeserializeObject<T>(cleaned);
    }
}