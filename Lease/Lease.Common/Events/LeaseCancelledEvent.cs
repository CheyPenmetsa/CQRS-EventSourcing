using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class LeaseCancelledEvent : BaseEvent
    {
        public LeaseCancelledEvent() : base(nameof(LeaseCancelledEvent))
        {
        }

        public string UserName { get; set; }

        public DateTime LeaseCancelledDate { get; set; }
    }
}
