using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Lease.Query.Infrastructure.Convertors;
using Lease.Query.Infrastructure.Handlers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Lease.Query.Infrastructure.Consumers
{
    public class KafkaEventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;

        private readonly IEventConsumerHandler _eventConsumerHandler;

        public KafkaEventConsumer(IOptions<ConsumerConfig> consumerConfig, 
            IEventConsumerHandler eventConsumerHandler)
        {
            _consumerConfig = consumerConfig.Value;
            _eventConsumerHandler = eventConsumerHandler;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message == null) continue;

                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
                var handlerMethod = _eventConsumerHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                if (handlerMethod == null)
                {
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
                }

                handlerMethod.Invoke(_eventConsumerHandler, new object[] { @event });
                //Will update Kafka commit log
                consumer.Commit(consumeResult);
            }
        }
    }
}
