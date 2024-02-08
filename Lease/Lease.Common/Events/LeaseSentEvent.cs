using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class LeaseSentEvent : BaseEvent
    {
        public LeaseSentEvent() : base(nameof(LeaseSentEvent))
        {
        }

        public DateTime LeaseSentDate { get; set; }
    }
}
