using Infra.External.Kafka.Consumer;

namespace WorkerService;

public class ConsumerWorker(IKafkaConsumer consumer, ILogger<ConsumerWorker> logger) : BackgroundService
{
    private readonly IKafkaConsumer _consumer = consumer;
    private readonly ILogger<ConsumerWorker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {@time}", DateTimeOffset.Now);
        await Task.Run(
            () => consumer.StartAsync(stoppingToken),
            stoppingToken);
    }
}
