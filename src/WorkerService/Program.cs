using Infra.External.Kafka;
using Infra.External.Kafka.Consumer;
using WorkerService;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaConsumerConfig>(
    builder.Configuration.GetSection("Kafka")
);

builder.Services.AddKafkaConsumer(builder.Configuration);
builder.Services.AddKafkaProcessor();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

builder.Services.AddHostedService<Worker>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
       )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var host = builder.Build();


host.Run();
