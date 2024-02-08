using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lease.Query.Infrastructure.Consumers
{
    public class KafkaTopicConsumerHostedService : IHostedService
    {
        private readonly ILogger<KafkaTopicConsumerHostedService> _logger;

        private readonly IServiceProvider _serviceProvider;

        public KafkaTopicConsumerHostedService(ILogger<KafkaTopicConsumerHostedService> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka topic consumer service running.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

                Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
            }

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka topic consumer service Stopped.");

            await Task.CompletedTask;
        }
    }
}
