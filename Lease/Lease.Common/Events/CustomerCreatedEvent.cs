using CQRS.Core.Events;

namespace Lease.Common.Events
{
    public class CustomerCreatedEvent : BaseEvent
    {
        public CustomerCreatedEvent() : base(nameof(CustomerCreatedEvent))
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
