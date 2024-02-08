using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class LeaseSignedEvent : BaseEvent
    {
        public LeaseSignedEvent() : base(nameof(LeaseSignedEvent))
        {
        }

        public string EmailAddress { get; set; }

        public DateTime LeaseSignedDate { get; set; }
    }
}
