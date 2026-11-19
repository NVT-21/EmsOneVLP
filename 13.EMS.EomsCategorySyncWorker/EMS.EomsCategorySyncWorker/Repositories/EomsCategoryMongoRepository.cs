using EMS.EomsCategorySyncWorker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EMS.EomsCategorySyncWorker.Repositories;

/// <summary>
/// Repository ghi log kết quả đồng bộ danh mục EOMS vào MongoDB.
/// </summary>
public class EomsCategoryMongoRepository
{
    private const string DefaultConnectionString = "mongodb://admin:ems14052026@192.168.86.16:27017/";
    private const string DefaultDatabaseName = "emsoneloguat";
    private const string DefaultCollectionName = "EomsCategorySyncLog";

    private static readonly TimeZoneInfo VietnamTz = ResolveVietnamTimeZone();

    private readonly IMongoCollection<EomsCategorySyncLog> _collection;
    private readonly ILogger<EomsCategoryMongoRepository> _logger;

    public EomsCategoryMongoRepository(
        IConfiguration configuration,
        ILogger<EomsCategoryMongoRepository> logger)
    {
        _logger = logger;

        var connectionString = FirstNonEmpty(
            configuration["EomsCategory:MongoConnectionString"],
            configuration["Mongo:ConnectionString"],
            DefaultConnectionString);

        var databaseName = FirstNonEmpty(
            configuration["EomsCategory:MongoDatabaseName"],
            configuration["Mongo:DatabaseName"],
            DefaultDatabaseName);

        var collectionName = FirstNonEmpty(
            configuration["EomsCategory:MongoCollectionName"],
            configuration["Mongo:EomsCategorySyncCollectionName"],
            DefaultCollectionName);

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<EomsCategorySyncLog>(collectionName);
    }

    /// <summary>
    /// Ghi 1 bản ghi log đồng bộ vào MongoDB; bắt mọi exception để không làm gián đoạn worker.
    /// </summary>
    public void InsertLog(EomsCategorySyncLog log)
    {
        if (log == null)
            return;

        try
        {
            var nowUtc7 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VietnamTz);
            log.Time = nowUtc7;
            log.TimeText = nowUtc7.ToString("yyyy-MM-dd HH:mm:ss");

            _collection.InsertOne(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "EomsCategoryMongoRepository.InsertLog failed. SyncId={SyncId}, Category={Category}, Action={Action}, ItemCode={ItemCode}",
                log.SyncId,
                log.Category,
                log.Action,
                log.ItemCode);
        }
    }

    private static string FirstNonEmpty(params string?[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
                return candidate.Trim();
        }

        return string.Empty;
    }

    private static TimeZoneInfo ResolveVietnamTimeZone()
    {
        foreach (var tzId in new[] { "SE Asia Standard Time", "Asia/Ho_Chi_Minh" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(tzId);
            }
            catch (TimeZoneNotFoundException)
            {
            }
        }

        return TimeZoneInfo.CreateCustomTimeZone("UTC+7", TimeSpan.FromHours(7), "Vietnam Standard Time", "Vietnam Standard Time");
    }
}
