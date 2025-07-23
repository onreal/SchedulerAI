using SchedulerApi.Domain.Shared;
using SchedulerApi.Domain.ValueObjects;

namespace SchedulerApi.Domain.User.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public Plan Plan { get; private set; }
    public string ApiKey { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int UsageCount { get; private set; } = 0;

    public User(string name, string surname, string email, Plan plan, string apiKey)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        Name = name ?? throw new ArgumentNullException(nameof(name));;
        Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Plan = plan;
        ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public void Update(string name, string surname, string email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));;
        Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void RegenerateApiKey(Func<string> apiKeyGenerator)
    {
        if (apiKeyGenerator == null) 
            throw new ArgumentNullException(nameof(apiKeyGenerator));

        ApiKey = apiKeyGenerator();
    }
    
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }
    
    public void SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Surname cannot be empty", nameof(surname));

        Surname = surname;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Email = email;
    }
    
    public void SetPlan(Plan plan)
    {
        Plan = plan;
    }
    
    public void SetApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API Key cannot be empty", nameof(apiKey));

        ApiKey = apiKey;
    }

    public static User Rehydrate(
        Guid id,
        string name,
        string surname,
        string email,
        Plan plan,
        string apiKey,
        DateTime createdAt,
        int usageCount
    )
    {
        var user = new User(name, surname, email, plan, apiKey);
        user.SetId(id); // private setter or protected method
        user.SetCreatedAt(createdAt);
        return user;
    }

    private void increaseUsageCount()
    {
        UsageCount++;
    }

    private void SetCreatedAt(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }
}