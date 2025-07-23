namespace SchedulerApi.Integrations.Generative.ChatGpt.DTOs;

public record ChatGptSettings(string ApiKey, string Model = "default");