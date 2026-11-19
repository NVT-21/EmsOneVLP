using System.Net.Http.Headers;
using EMS.EomsCategorySyncWorker.Models;
using EMS.EomsCategorySyncWorker.Repositories;
using EMS.EomsCategorySyncWorker.Services;
using EMS.EomsCategorySyncWorker.Workers;
using Microsoft.Extensions.Hosting;

Directory.SetCurrentDirectory(AppContext.BaseDirectory);

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "EMS.EomsCategorySyncWorker";
});

var httpTimeoutSeconds = builder.Configuration.GetValue("Worker:HttpTimeoutSeconds", 120);
if (httpTimeoutSeconds < 5)
    httpTimeoutSeconds = 5;

builder.Services.AddHttpClient("EomsCategory", client =>
{
    client.Timeout = TimeSpan.FromSeconds(httpTimeoutSeconds);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

var stopHostOnBgException = builder.Configuration.GetValue("Worker:StopHostOnBackgroundServiceException", true);
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = stopHostOnBgException
        ? BackgroundServiceExceptionBehavior.StopHost
        : BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.Configure<EomsCategoryOptions>(builder.Configuration.GetSection("EomsCategory"));
builder.Services.AddSingleton<EomsCategoryClient>();
builder.Services.AddSingleton<EomsCategoryMongoRepository>();
builder.Services.AddSingleton<EomsCategoryRepository>();
builder.Services.AddHostedService<EomsCategorySyncWorker>();

var host = builder.Build();

host.Run();
