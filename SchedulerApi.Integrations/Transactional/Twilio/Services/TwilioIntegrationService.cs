using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SchedulerApi.Domain.UserIntegration;
using SchedulerApi.Integrations.Transactional.Twilio.Contracts;
using SchedulerApi.Integrations.Transactional.Twilio.DTOs;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SchedulerApi.Integrations.Transactional.Twilio.Services;

public class TwilioIntegrationService : ITwilioIntegrationService
{
    public string Name => "Twilio";
    private string _fromPhoneNumber;

    public async Task HandleAsync(UserIntegration userIntegration, IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var settings = JsonConvert.DeserializeObject<TwilioSettings>(
            (userIntegration != null 
                ? userIntegration.Settings 
                : configuration.GetSection("Integrations:Twilio").Value) ?? string.Empty);
        
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "Twilio settings cannot be null.");
        }
        
        TwilioClient.Init(settings.AccountSid, settings.AuthToken);
        _fromPhoneNumber = settings.FromPhoneNumber;
    }
    
    public async Task SendMessageAsync(string toPhoneNumber, string message, CancellationToken cancellationToken = default)
    {
        await MessageResource.CreateAsync(
            to: new PhoneNumber(toPhoneNumber),
            from: new PhoneNumber(_fromPhoneNumber),
            body: message
        );
    }
}