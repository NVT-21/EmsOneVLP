using EMS.Core;
using EMS.Library;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Core
{
    public static class XRedisManager
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
        new Lazy<ConnectionMultiplexer>(() =>
        {
            try
            {
                var connStr = XHelper.ConnectionStringRedis;
                if(string.IsNullOrWhiteSpace(connStr))
                {
                    throw new Exception("ConnectionStringRedis is null or empty");
                }

                var options = ConfigurationOptions.Parse(connStr);
                options.AbortOnConnectFail = false;
                options.ConnectTimeout = 5000;
                options.SyncTimeout = 5000;

                return ConnectionMultiplexer.Connect(options);
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(
                    LogFileType.EXCEPTION,
                    "XRedisManager init error: " + ex.ToString());

                throw;
            }
        });

        public static ConnectionMultiplexer Connection
        {
            get { return LazyConnection.Value; }
        }

        public static IDatabase Db
        {
            get { return Connection.GetDatabase(); }
        }
    }
}