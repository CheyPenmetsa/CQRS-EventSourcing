using Lease.Common.Events;

namespace Lease.Query.Infrastructure.Handlers
{
    public interface IEventConsumerHandler
    {
        Task On(LeaseCreatedEvent @event);

        Task On(LeaseSentEvent @event);

        Task On(LeaseSignedEvent @event);

        Task On(LeaseEditedEvent @event);

        Task On(LeaseCancelledEvent @event);
    }
}
