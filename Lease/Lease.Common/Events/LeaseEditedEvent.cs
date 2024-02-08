using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class LeaseEditedEvent : BaseEvent
    {
        public LeaseEditedEvent() : base(nameof(LeaseEditedEvent))
        {
        }
        public string ParkingSpace { get; set; }

        public string ApartmentNumber { get; set; }

        public DateTime LeaseEditedDate { get; set; }
    }
}
