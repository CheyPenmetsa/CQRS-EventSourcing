using CQRS.Core.Domain;
using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventsAsync<T>(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion) where T : AggregateRoot;

        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);

        Task<List<Guid>> GetAggregateIdsAsync();

        Task PublishEventAsync(BaseEvent @event);
    }
}
