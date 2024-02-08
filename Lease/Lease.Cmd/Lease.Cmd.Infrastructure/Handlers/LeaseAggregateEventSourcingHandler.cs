using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Lease.Cmd.Domain.Aggregates;

namespace Lease.Cmd.Infrastructure.Handlers
{
    public class LeaseAggregateEventSourcingHandler : IEventSourcingHandler<LeaseAggregate>
    {
        private readonly IEventStore _eventStore;

        public LeaseAggregateEventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync<LeaseAggregate>(aggregate.Id,
                aggregate.GetUncommittedChanges(),
                aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }

        public async Task<LeaseAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new LeaseAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null || !events.Any())
                return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();
            return aggregate;
        }

        public async Task ReplayEventsAsync(Guid aggregateId)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate == null || aggregate.IsCancelled)
                return;

            var events = await _eventStore.GetEventsAsync(aggregateId);
            foreach (var @event in events)
            {
                await _eventStore.PublishEventAsync(@event);
            }
        }
    }
}
