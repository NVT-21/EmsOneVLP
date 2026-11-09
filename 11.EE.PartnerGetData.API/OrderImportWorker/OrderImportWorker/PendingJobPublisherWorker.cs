using OrderImportWorker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker
{
    public class PendingJobPublisherWorker : BackgroundService
    {
        private readonly PendingJobQueuePublisher _publisher;
        private readonly ILogger<PendingJobPublisherWorker> _logger;

        public PendingJobPublisherWorker(
            PendingJobQueuePublisher publisher,
            ILogger<PendingJobPublisherWorker> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PendingJobPublisherWorker started");
            await _publisher.ScanAndPublishPendingJobsAsync(stoppingToken);
        }
    }
}
