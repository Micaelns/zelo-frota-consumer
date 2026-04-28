namespace Infra.External.Kafka.Consumer;

public interface IKafkaConsumer
{
    Task StartAsync(CancellationToken cancellationToken);
}
