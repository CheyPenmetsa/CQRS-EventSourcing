using CQRS.Core.Messages;

namespace CQRS.Core.Events
{
    public abstract class BaseEvent : Message
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }

        //We will use the version for replaying the latest state of the aggregate
        public int Version { get; set; }

        // This will be used for deserializing objects in case of Kafka
        public string Type { get; set; }
    }
}
