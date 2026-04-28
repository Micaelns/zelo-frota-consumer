using Application.Processors;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infra.External.Kafka.Consumer;

public class KafkaConsumer(
    IOptions<KafkaConsumerConfig> config,
    ILogger<KafkaConsumer> logger,
    IServiceProvider serviceProvider) : IKafkaConsumer
{
    private readonly ILogger<KafkaConsumer> _logger = logger;
    private readonly KafkaConsumerConfig _config = config.Value;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _config.BootstrapServers,
            GroupId = _config.ConsumerGroup,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

        consumer.Subscribe(_config.Topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(cancellationToken);

                if (result?.Message == null)
                    continue;

                await ProcessMessageAsync(result.Message.Value, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro durante o consumo de mensagem: {@error}", ex.Message);
            }
        }
    }

    private async Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var processor = scope.ServiceProvider.GetRequiredService<IEventProcessor>();

        await processor.ProcessAsync(message, cancellationToken);
    }
}
