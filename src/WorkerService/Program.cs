using Infra.Extensions;
using Infra.External.Kafka;
using Infra.External.Kafka.Consumer;
using Serilog;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaConsumerConfig>(
    builder.Configuration.GetSection("Kafka")
);

builder.Services.AddKafkaConsumer(builder.Configuration);
builder.Services.AddKafkaProcessor();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
builder.Services.AddExcelDependence();

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

var sqlQueryString = builder.Configuration["connectionStringSqlServer"];
builder.Services.AddContexts(sqlQueryString);
builder.Services.ImplementsRepository();

var host = builder.Build();


host.Run();
