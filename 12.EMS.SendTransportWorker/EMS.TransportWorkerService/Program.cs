using EMS.TransportWorkerService;
using EMS.TransportWorkerService.Repositories;
using EMS.TransportWorkerService.Workers;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var httpTimeoutSeconds = builder.Configuration.GetValue("Worker:HttpTimeoutSeconds", 120);
if (httpTimeoutSeconds < 5)
    httpTimeoutSeconds = 5;

void ConfigureHttpClient(HttpClient client)
{
    client.Timeout = TimeSpan.FromSeconds(httpTimeoutSeconds);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
    );
}

builder.Services.AddHttpClient("EMSClient", ConfigureHttpClient);
builder.Services.AddHttpClient(string.Empty, ConfigureHttpClient);

var stopHostOnBgException = builder.Configuration.GetValue("Worker:StopHostOnBackgroundServiceException", true);
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = stopHostOnBgException
        ? BackgroundServiceExceptionBehavior.StopHost
        : BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddSingleton<APILibraryRepository>();
builder.Services.AddSingleton<PushPNSEMSMongoRepository>();
builder.Services.AddSingleton<PushPNSVNPOSTMongoRepository>();
builder.Services.AddSingleton<EMSRepository>();
builder.Services.AddSingleton<PNSVnpostRepository>();
builder.Services.AddSingleton<TransportQueueMongoRepository>();
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<TransportRabbitPublisher>();
builder.Services.AddHostedService<TransportMongoPublisherWorker>();
builder.Services.AddHostedService<RabbitTransportConsumerWorker>();

var host = builder.Build();

host.Run();
