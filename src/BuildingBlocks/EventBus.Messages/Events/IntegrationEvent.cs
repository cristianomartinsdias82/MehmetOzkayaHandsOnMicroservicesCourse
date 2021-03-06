using System;

namespace EventBus.Messages.Events
{
    public class IntegrationEvent
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }

        public IntegrationEvent() : this(Guid.NewGuid(), DateTime.UtcNow) {}

        public IntegrationEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}