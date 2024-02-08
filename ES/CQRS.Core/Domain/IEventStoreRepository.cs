using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    //Note: we wont have update or delete since we will always add to the event store
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @event);

        Task<List<EventModel>> FindEventsByAggregateId(Guid aggregateId);

        Task<List<EventModel>> FindAllEventsAsync();
    } 
}
