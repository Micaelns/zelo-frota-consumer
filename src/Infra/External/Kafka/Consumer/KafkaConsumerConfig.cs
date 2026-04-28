namespace Infra.External.Kafka.Consumer;

public class KafkaConsumerConfig
{
    public string BootstrapServers { get; set; } = default!;
    public string ConsumerGroup { get; set; } = default!;
    public string Topic { get; set; } = default!;
}
