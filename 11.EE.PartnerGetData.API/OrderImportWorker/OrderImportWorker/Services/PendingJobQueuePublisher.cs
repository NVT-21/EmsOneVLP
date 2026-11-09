using OrderImportWorker.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Services
{
    public class PendingJobQueuePublisher
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly RabbitMqPublisher _publisher;
        private readonly ILogger<PendingJobQueuePublisher> _logger;

        public PendingJobQueuePublisher(
            RedisConnectionFactory redisFactory,
            RabbitMqPublisher publisher,
            ILogger<PendingJobQueuePublisher> logger)
        {
            _redis = redisFactory.GetConnection();
            _db = redisFactory.GetDatabase();
            _publisher = publisher;
            _logger = logger;
        }

        public async Task ScanAndPublishPendingJobsAsync(CancellationToken cancellationToken)
        {
            while( !cancellationToken.IsCancellationRequested )
            {
                try
                {
                    await ScanPendingJobsOnceAsync(cancellationToken);
                }
                catch( Exception ex )
                {
                    _logger.LogError(ex, "Scan pending jobs failed");
                }

                await Task.Delay(3000, cancellationToken);
            }
        }

        private async Task ScanPendingJobsOnceAsync(CancellationToken cancellationToken)
        {
            var endpoints = _redis.GetEndPoints();
            if( endpoints == null || endpoints.Length == 0 )
                return;

            var server = _redis.GetServer(endpoints[0]);

            await foreach( var redisKey in server.KeysAsync(
                database: _db.Database,
                pattern: "import:job:*",
                pageSize: 200) )
            {
                if( cancellationToken.IsCancellationRequested )
                    break;

                var key = redisKey.ToString();

                if( !IsMainJobKey(key) )
                    continue;

                await TryPublishJobAsync(key, cancellationToken);
            }
        }

        private bool IsMainJobKey(string key)
        {
            var parts = key.Split(':');
            return parts.Length == 3 && parts[0] == "import" && parts[1] == "job";
        }

        private async Task TryPublishJobAsync(string jobKey, CancellationToken cancellationToken)
        {
            var entries = await _db.HashGetAllAsync(jobKey);
            if( entries == null || entries.Length == 0 )
                return;

            string Get(string field)
            {
                foreach( var x in entries )
                {
                    if( x.Name == field ) return x.Value!;
                }
                return "";
            }

            var status = Get("status");
            if( !string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase) )
                return;

            var jobIdText = Get("jobId");
            if( string.IsNullOrWhiteSpace(jobIdText) )
                return;

            // lock tránh nhiều instance cùng publish 1 job
            string lockKey = $"import:job:{jobIdText}:enqueue-lock";
            bool gotLock = await _db.StringSetAsync(
                lockKey,
                "1",
                TimeSpan.FromSeconds(30),
                when: When.NotExists);

            if( !gotLock )
                return;

            try
            {
                // đọc lại sau khi lock
                entries = await _db.HashGetAllAsync(jobKey);
                if( entries == null || entries.Length == 0 )
                    return;

                status = "";
                foreach( var x in entries )
                {
                    if( x.Name == "status" )
                    {
                        status = x.Value!;
                        break;
                    }
                }

                if( !string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase) )
                    return;

                int customerId = 0;
                int.TryParse(Get("shopId"), out customerId);

                var message = new OrderImportJobMessage
                {
                    JobId = long.Parse(jobIdText),
                    CustomerId = customerId,
                    FileHash = Get("fileHash"),
                    FileName = Get("fileName"),
                    CreatedAt = Get("createdAt")
                };

                // chuyển sang Processing ngay lúc đưa vào hàng chờ
                await _db.HashSetAsync(jobKey, new HashEntry[]
                {
                    new HashEntry("status", "Processing")
                });

                try
                {
                    await _publisher.PublishAsync(message, cancellationToken);
                    _logger.LogInformation("Published job {JobId} to RabbitMQ and set status=Processing", jobIdText);
                }
                catch
                {
                    // publish lỗi thì trả lại Pending
                    await _db.HashSetAsync(jobKey, new HashEntry[]
                    {
                        new HashEntry("status", "Pending")
                    });

                    throw;
                }
            }
            finally
            {
                await _db.KeyDeleteAsync(lockKey);
            }
        }
    }
}
