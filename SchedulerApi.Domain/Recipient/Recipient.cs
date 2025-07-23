using SchedulerApi.Domain.Shared;

namespace SchedulerApi.Domain.Recipient;

public class Recipient : Entity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public Recipient(Guid userId, string name, string surname, string phone, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Surname cannot be empty", nameof(surname));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty", nameof(phone));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        UserId = userId;
        Name = name;
        Surname = surname;
        Phone = phone;
        Email = email;
    }
    
    public static Recipient Create(string name, string surname, string phone, string email)
    {
        return new Recipient(Guid.Empty, name, surname, phone, email)
        {
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public void Update(string name, string surname, string phone, string email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    private void SetCreatedAt(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }
    
    public static Recipient Rehydrate(
        Guid id,
        Guid userId, 
        string name,
        string surname,
        string phone,
        string email,
        DateTime createdAt
    )
    {
        var recipients = new Recipient(
            userId,
            name,
            surname,
            phone,
            email
        );
        recipients.SetId(id);
        recipients.SetCreatedAt(createdAt);

        return recipients;
    }
}