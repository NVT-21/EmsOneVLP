using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Models
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }

    public class RedisOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class ImportOptions
    {
        public int MaxRowConcurrency { get; set; } = 30;
    }

    public class ApiSyncOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
