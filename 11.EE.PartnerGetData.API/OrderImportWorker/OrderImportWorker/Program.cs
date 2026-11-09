using OrderImportWorker;
using OrderImportWorker.Models;
using OrderImportWorker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.Configure<ImportOptions>(builder.Configuration.GetSection("Import"));
builder.Services.Configure<ApiSyncOptions>(builder.Configuration.GetSection("ApiSync"));

builder.Services.AddSingleton<RedisConnectionFactory>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<AddressApiService>();
builder.Services.AddSingleton<ImportJobProcessor>();
builder.Services.AddSingleton<PendingJobQueuePublisher>();
builder.Services.AddSingleton<RabbitMqImportConsumer>();

builder.Services.AddHostedService<PendingJobPublisherWorker>();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
