using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        // This is document id not aggregateid
        public string Id { get; set; }

        public DateTime DocTimeStamp { get; set; }

        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        public string EventType { get; set; }

        public BaseEvent EventData { get; set; }
    }
}
