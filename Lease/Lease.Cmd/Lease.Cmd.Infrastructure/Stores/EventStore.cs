using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;

namespace Lease.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, 
            IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            var events = await _eventStoreRepository.FindAllEventsAsync();

            if(events == null || !events.Any())
            {
                throw new ArgumentNullException("Could not find any events");
            }

            return events.Select(e => e.AggregateId).Distinct().ToList();
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var events = await _eventStoreRepository.FindEventsByAggregateId(aggregateId);

            if (events == null || !events.Any())
            {
                throw new AggregateNotFoundException($"LeaseId not found for {aggregateId}");
            }

            return events.OrderBy(x=>x.Version).Select(x=>x.EventData).ToList();
        }

        public async Task PublishEventAsync(BaseEvent @event)
        {
            // Environment variable can be read based on where it is deployed, for local you can set this in launchSettings.json file
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            await _eventProducer.ProduceEventAsync(topic, @event);
        }

        //NOTE: In below code if event got persisted but the event did not get published then write and read databases will be out of sync
        // So it is better to implement that in a transaction, which we are not going to in this solution.
        public async Task SaveEventsAsync<T>(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion) where T : AggregateRoot
        {
            var aggEvents = await _eventStoreRepository.FindEventsByAggregateId(aggregateId);

            if (expectedVersion != -1 && aggEvents[^1].Version != expectedVersion)
            {
                throw new InvalidDataStateException();
            }

            var currentVersion = expectedVersion;

            foreach (var @event in events)
            {
                currentVersion++;
                @event.Version = currentVersion;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel()
                {
                    DocTimeStamp = DateTime.UtcNow,
                    AggregateId = aggregateId,
                    EventType = eventType,
                    AggregateType = nameof(T),
                    Version = currentVersion,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                await PublishEventAsync(@event);
            }
        }
    }
}
