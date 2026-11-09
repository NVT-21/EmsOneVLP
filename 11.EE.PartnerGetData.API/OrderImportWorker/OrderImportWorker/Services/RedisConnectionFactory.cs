using Microsoft.Extensions.Options;
using OrderImportWorker.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Services
{
    public class RedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;

        public RedisConnectionFactory(IOptions<RedisOptions> redisOptions)
        {
            _connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
            });
        }
        public IConnectionMultiplexer GetConnection()
        {
            return _connection.Value;
        }
        public IDatabase GetDatabase()
        {
            return _connection.Value.GetDatabase();
        }
    }
}
