using EMS.Entity;
using EMS.Library;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EMS.API.PartnerGateway
{
    public static class XMongoLogHelper
    {
        private static readonly BlockingCollection<ZApiLogRequest> _queue =
            new BlockingCollection<ZApiLogRequest>(5000);

        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private static readonly Task _worker;

        private static readonly IMongoDatabase _database;
        static XMongoLogHelper()
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["MONGO_LOG_CONNECTION"];
                string databaseName = ConfigurationManager.AppSettings["MONGO_LOG_DATABASE"];

                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(2);
                settings.ConnectTimeout = TimeSpan.FromSeconds(2);
                settings.SocketTimeout = TimeSpan.FromSeconds(2);

                var client = new MongoClient(settings);
                _database = client.GetDatabase(databaseName);

                _worker = Task.Factory.StartNew(
                    ProcessQueue,
                    _cts.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }
            catch
            {
            }
        }
        public static void Enqueue(ZApiLogRequest log)
        {
            try
            {
                if(log == null)
                    return;

                if(string.IsNullOrWhiteSpace(log.CollectionName))
                {
                    log.CollectionName = "EmsOneErrorLog";
                }

                if(!_queue.TryAdd(log))
                {
                    SafeFallbackToFile("[QueueFull] " + log.Code);
                }
            }
            catch
            {
            }
        }

        private static void ProcessQueue()
        {
            while(!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    ZApiLogRequest item;
                    if(!_queue.TryTake(out item, 1000))
                        continue;

                    if(_database == null)
                    {
                        SafeFallbackToFile("[MongoDatabaseNull] " + (item != null ? item.Code : ""));
                        continue;
                    }

                    string collectionName = item != null && !string.IsNullOrWhiteSpace(item.CollectionName)
                        ? item.CollectionName
                        : "EmsOneErrorLog";

                    var collection = _database.GetCollection<ZApiLogRequest>(collectionName);
              
                    collection.InsertOne(item);
                }
                catch(Exception ex)
                {
                    SafeFallbackToFile("[MongoInsertError] " + ex.Message);
                }
            }
        }

        private static void SafeFallbackToFile(string message)
        {
            try
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, message);
            }
            catch
            {
            }
        }

        public static void Shutdown()
        {
            try
            {
                _cts.Cancel();
                _queue.CompleteAdding();

                if(_worker != null)
                {
                    _worker.Wait(1000);
                }
            }
            catch
            {
            }
        }

        public static string GetCollectionName(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
                return "EmsOneErrorLog";

            switch(code)
            {
                case "PARTNER_ORDER_ADD":
                    return "PartnerOrderAdd";

                case "PARTNER_VIB_ORDER_ADD":
                    return "PartnerOrderAddVIB";

                case "PARTNER_HM_ORDER_ADD":
                    return "PartnerOrderAddHM";

                case "PARTNER_ADDRESS_SEARCH":
                    return "PartnerAnalysticAddress";

                case "PARTNER_ANALYSTIC_ORDER_ADD":
                    return "PartnerOrderAddAnalystic";

                default:
                    return "EmsOneErrorLog";
            }
        }

        public static bool ShouldLog(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
                return false;

            switch(code)
            {
                case "PARTNER_ORDER_ADD":
                case "PARTNER_VIB_ORDER_ADD":
                case "PARTNER_HM_ORDER_ADD":
                case "PARTNER_ADDRESS_SEARCH":
                    return true;

                default:
                    return false;
            }
        }
    }
}