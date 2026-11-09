using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderImportWorker.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Services
{
    public class ImportJobProcessor
    {
        private readonly IDatabase _db;
        private readonly AddressApiService _addressApiService;
        private readonly ImportOptions _importOptions;
        private readonly ILogger<ImportJobProcessor> _logger;

        public ImportJobProcessor(
            RedisConnectionFactory redisFactory,
            AddressApiService addressApiService,
            IOptions<ImportOptions> importOptions,
            ILogger<ImportJobProcessor> logger)
        {
            _db = redisFactory.GetDatabase();
            _addressApiService = addressApiService;
            _importOptions = importOptions.Value;
            _logger = logger;
        }

        public async Task Process(OrderImportJobMessage job)
        {
            string jobKey = $"import:job:{job.JobId}";
            string rowsKey = $"import:job:{job.JobId}:rows";

            var rowKeys = _db.SetMembers(rowsKey)
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            int successRows = 0;
            int failedRows = 0;

            int maxConcurrency = _importOptions.MaxRowConcurrency <= 0 ? 30 : _importOptions.MaxRowConcurrency;

            _logger.LogInformation("Processing Job {JobId} with concurrency {Concurrency}", job.JobId, maxConcurrency);

            var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = new List<Task>();

            foreach( var rowKey in rowKeys )
            {
                await semaphore.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        bool ok = await ProcessRow(job, rowKey);
                        if( ok )
                            Interlocked.Increment(ref successRows);
                        else
                            Interlocked.Increment(ref failedRows);
                    }
                    catch( Exception ex )
                    {
                        Interlocked.Increment(ref failedRows);

                        _db.HashSet(rowKey, new HashEntry[]
                        {
                            new HashEntry("parseStatus", "3"),
                            new HashEntry("parseError", ex.Message),
                            new HashEntry("processedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        });

                        _logger.LogError(ex, "Process row failed: {RowKey}", rowKey);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            string finalStatus = failedRows > 0 ? "Completed" : "Completed";

            _db.HashSet(jobKey, new HashEntry[]
            {
                new HashEntry("status", finalStatus),
                new HashEntry("successRows", successRows),
                new HashEntry("failedRows", failedRows),
                new HashEntry("completedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            });

            _logger.LogInformation("Completed Job {JobId} Success={SuccessRows} Failed={FailedRows}",
                job.JobId, successRows, failedRows);
        }

        private async Task<bool> ProcessRow(OrderImportJobMessage job, string rowKey)
        {
            var entries = _db.HashGetAll(rowKey);

            Func<string, string> get = key =>
            {
                foreach( var x in entries )
                {
                    if( x.Name == key ) return x.Value!;
                }
                return "";
            };

            _db.HashSet(rowKey, "parseStatus", "1");

            string orderCode = get("OrderCode");
            string receiverStreet = get("ReceiverStreet");
            string receiverWardName = get("ReceiverWardName");
            string receiverProvinceName = get("ReceiverProvinceName");

            string senderStreet = get("SenderStreet");
            string senderWardName = get("SenderWardName");
            string senderProvinceName = get("SenderProvinceName");

            string rowHash = XHashHelper.ComputeSha256Text(
                $"{orderCode}|{receiverStreet}|{receiverWardName}|{receiverProvinceName}|{senderStreet}|{senderWardName}|{senderProvinceName}");

            string cacheKey = $"import:parse:{job.CustomerId}:{job.FileHash}:{rowHash}";

            AddressParseResult? result = null;
            var cached = _db.StringGet(cacheKey);

            if( cached.HasValue )
            {
                result = JsonConvert.DeserializeObject<AddressParseResult>(cached!);
            }

            if( result == null )
            {
                result = await _addressApiService.ParseAddressAsync(new AddressParseRequest
                {
                    ReceiverStreet = receiverStreet,
                    ReceiverWardName = receiverWardName,
                    ReceiverProvinceName = receiverProvinceName,
                    SenderStreet = senderStreet,
                    SenderWardName = senderWardName,
                    SenderProvinceName = senderProvinceName
                });

                _db.StringSet(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromHours(24));
            }

            _db.HashSet(rowKey, new HashEntry[]
            {
                new HashEntry("ReceiverProvinceID", result.ReceiverProvinceID ?? ""),
                new HashEntry("ReceiverWardID", result.ReceiverWardID ?? ""),
                new HashEntry("SenderProvinceID", result.SenderProvinceID ?? ""),
                new HashEntry("SenderWardID", result.SenderWardID ?? ""),
                new HashEntry("parseStatus", result.IsSuccess ? "2" : "3"),
                new HashEntry("parseError", result.ErrorMessage ?? ""),
                new HashEntry("processedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            });

            return result.IsSuccess;
        }
    }
}
