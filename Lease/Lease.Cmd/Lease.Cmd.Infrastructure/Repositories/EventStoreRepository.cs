using CQRS.Core.Domain;
using CQRS.Core.Events;
using Lease.Cmd.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Lease.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<EventStoreDatabaseSettings> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDbName = mongoClient.GetDatabase(config.Value.DatabaseName);

            _eventStoreCollection = mongoDbName.GetCollection<EventModel>(config.Value.CollectionName);
        }

        public async Task<List<EventModel>> FindAllEventsAsync()
        {
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindEventsByAggregateId(Guid aggregateId)
        {
            // ConfigureAwait(false) will avoid from forcing the callback from origination
            return await _eventStoreCollection.Find(x => x.AggregateId == aggregateId).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}
