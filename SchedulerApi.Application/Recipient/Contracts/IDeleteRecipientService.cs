namespace SchedulerApi.Application.Recipient.Contracts;

public interface IDeleteRecipientService
{
    Task DeleteAsync(Guid id);
}