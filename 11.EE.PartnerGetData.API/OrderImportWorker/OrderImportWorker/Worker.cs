using OrderImportWorker.Services;

namespace OrderImportWorker
{
    public class Worker : BackgroundService
    {
        private readonly RabbitMqImportConsumer _consumer;
        private readonly ILogger<Worker> _logger;

        public Worker(RabbitMqImportConsumer consumer, ILogger<Worker> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderImportWorker started");

            await _consumer.StartAsync(stoppingToken);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch( TaskCanceledException )
            {
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderImportWorker stopping");
            await _consumer.StopAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
