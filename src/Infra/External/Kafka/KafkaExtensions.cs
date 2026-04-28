using Application.Processors;
using Infra.External.Kafka.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.External.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaConsumer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

        return services;
    }

    public static IServiceCollection AddKafkaProcessor(
        this IServiceCollection services)
    {
        services.AddScoped<IEventProcessor, TravelReportProcessor>();

        return services;
    }
}
