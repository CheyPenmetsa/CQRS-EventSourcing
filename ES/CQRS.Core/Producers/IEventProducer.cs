using CQRS.Core.Events;

namespace CQRS.Core.Producers
{
    public interface IEventProducer
    {
        Task ProduceEventAsync<T>(string topic, T @event) where T : BaseEvent;
    }
}
