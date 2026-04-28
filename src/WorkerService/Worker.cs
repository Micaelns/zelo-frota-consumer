using Infra.External.Kafka.Consumer;

namespace WorkerService;

public class Worker(IKafkaConsumer consumer, ILogger<Worker> logger) : BackgroundService
{
    private readonly IKafkaConsumer _consumer = consumer;
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {@time}", DateTimeOffset.Now);
        await _consumer.StartAsync(stoppingToken);
    }
}
