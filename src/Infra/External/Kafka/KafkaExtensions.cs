using Application.Contracts;
using Application.Processors;
using Infra.External.Excel;
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

    public static IServiceCollection AddExcelDependence(
        this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, LocalFileStorage>();
        services.AddScoped<IExcelService, ExcelService>();

        return services;
    }
}
