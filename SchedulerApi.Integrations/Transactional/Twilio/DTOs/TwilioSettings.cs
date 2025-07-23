namespace SchedulerApi.Integrations.Transactional.Twilio.DTOs;

public record TwilioSettings(string AccountSid, string AuthToken, string FromPhoneNumber);