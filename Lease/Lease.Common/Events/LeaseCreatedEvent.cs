using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class LeaseCreatedEvent : BaseEvent
    {
        public LeaseCreatedEvent() : base(nameof(LeaseCreatedEvent))
        {
        }

        public List<string> CustomerEmails { get; set; }

        public string FloorPlan { get; set; }

        public DateTime LeaseStartDate { get; set; }

        public int LeaseTermLengthInMonths { get; set; }

        public DateTime LeaseCreatedDate { get; set; }

        public double RentAmount { get; set; }

        public string ParkingSpace { get; set; }

        public string ApartmentNumber { get; set; }
    }
}
