using SchedulerApi.Domain.Shared;

namespace SchedulerApi.Domain.Schedule
{
    public class Schedule : Entity
    {
        public Guid UserId { get; private set; }
        public List<string> IntegrationIds { get; private set; }
        public DateTime EventTime { get; private set; }
        public int NotifyBeforeMinutes { get; private set; }
        public List<Guid> RecipientIds { get; private set; }
        public bool IsSent { get; private set; }
        public DateTime? SentAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Schedule(Guid userId, DateTime eventTime, int notifyBeforeMinutes,
            List<Guid>? recipientIds, List<string> integrationIds)
        {
            if (eventTime <= DateTime.UtcNow)
                throw new ArgumentException("Event time must be in the future", nameof(eventTime));

            if (notifyBeforeMinutes <= 0)
                throw new ArgumentException("NotifyBeforeMinutes must be positive", nameof(notifyBeforeMinutes));

            if (recipientIds == null || !recipientIds.Any())
                throw new ArgumentException("Recipients cannot be empty", nameof(recipientIds));

            UserId = userId;
            EventTime = eventTime;
            NotifyBeforeMinutes = notifyBeforeMinutes;
            RecipientIds = recipientIds;
            IntegrationIds = integrationIds ?? new List<string>();
            IsSent = false;
            CreatedAt = DateTime.UtcNow;
        }

        public bool MarkAsSent()
        {
            SentAt = DateTime.UtcNow;
            return IsSent = true;
        }
        
        public static Schedule Create(
            DateTime eventTime,
            int notifyBeforeMinutes,
            List<Guid>? recipientIds,
            List<string> integrationIds
        )
        {
            return new Schedule(Guid.Empty, eventTime, notifyBeforeMinutes, recipientIds, integrationIds);
        }

        public void Update(DateTime eventTime, int notifyBeforeMinutes, Guid templateId, List<Guid> recipientIds,
            List<string> integrationIds)
        {
            if (eventTime <= DateTime.UtcNow)
                throw new ArgumentException("Event time must be in the future", nameof(eventTime));

            if (notifyBeforeMinutes <= 0)
                throw new ArgumentException("NotifyBeforeMinutes must be positive", nameof(notifyBeforeMinutes));

            if (recipientIds == null || !recipientIds.Any())
                throw new ArgumentException("Recipients cannot be empty", nameof(recipientIds));

            EventTime = eventTime;
            NotifyBeforeMinutes = notifyBeforeMinutes;
            RecipientIds = recipientIds;
            IntegrationIds = integrationIds;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetIntegrationIds(List<string> integrationIds)
        {
            if (integrationIds == null || !integrationIds.Any())
                throw new ArgumentException("Integration IDs cannot be empty", nameof(integrationIds));

            IntegrationIds = integrationIds;
        }
        
        private void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        private void SetUpdatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        private void SetIsSent(bool isSent)
        {
            IsSent = isSent;
        }
        
        private void SetSentAt(DateTime? sentAt)
        {
            SentAt = sentAt;
        }
        
        public static Schedule Rehydrate(
            Guid id,
            Guid userId,
            List<string>? integrationIds,
            DateTime eventTime,
            int notifyBeforeMinutes,
            List<Guid> recipientIds,
            bool isSent,
            DateTime? sentAt,
            DateTime createdAt,
            DateTime updatedAt
        )
        {
            var scheduler = new Schedule(
                userId,
                eventTime,
                notifyBeforeMinutes,
                recipientIds,
                integrationIds ?? new List<string>()
            );
            
            scheduler.SetId(id);
            scheduler.SetCreatedAt(createdAt);
            scheduler.SetUpdatedAt(updatedAt);
            scheduler.SetIsSent(isSent);
            scheduler.SetSentAt(sentAt);

            return scheduler;
        }
    }
}