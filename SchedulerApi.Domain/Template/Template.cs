using SchedulerApi.Domain.Shared;

namespace SchedulerApi.Domain.Template
{
    public class Template : Entity
    {
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Template(Guid userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is required", nameof(message));

            UserId = userId;
            Message = message;
        }
        
        public static Template Create(string message)
        {
            return new Template(Guid.Empty, message)
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void SetMessage(string newMessage)
        {
            if (string.IsNullOrWhiteSpace(newMessage))
                throw new ArgumentException("Message cannot be empty", nameof(newMessage));

            Message = newMessage;
        }

        public void Update(string newMessage)
        {
            if (string.IsNullOrWhiteSpace(newMessage))
                throw new ArgumentException("Message is required", nameof(newMessage));
            
            Message = newMessage;
            UpdatedAt = DateTime.UtcNow;
        }
        
        private void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        private void SetUpdatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        public static Template Rehydrate(
            Guid id,
            Guid userId,
            string message,
            DateTime createdAt,
            DateTime updatedAt
        )
        {
            var user = new Template(
                userId,
                message
                );
            user.SetId(id);
            user.SetCreatedAt(createdAt);
            user.SetUpdatedAt(updatedAt);
            return user;
        }
    }
}