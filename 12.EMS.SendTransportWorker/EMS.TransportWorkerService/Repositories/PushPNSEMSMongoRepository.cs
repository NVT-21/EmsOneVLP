using EMS.TransportWorkerService.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EMS.TransportWorkerService.Repositories;

public class PushPNSEMSMongoRepository
{
    private static readonly TimeZoneInfo VietnamTz = ResolveVietnamTimeZone();

    private readonly IMongoCollection<PushPNSEMSLog> _collection;
    private readonly ILogger<PushPNSEMSMongoRepository> _logger;

    public PushPNSEMSMongoRepository(
        IConfiguration configuration,
        ILogger<PushPNSEMSMongoRepository> logger)
    {
        _logger = logger;

        var connectionString = configuration["Mongo:ConnectionString"];
        var databaseName = configuration["Mongo:DatabaseName"];
        var collectionName = configuration["Mongo:PushPNSEMSCollectionName"];
        if (string.IsNullOrWhiteSpace(collectionName))
            collectionName = "PushPNSEMS";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<PushPNSEMSLog>(collectionName);
    }

    public void InsertLog(string request, string response, string itemCode, string code, bool success)
    {
        try
        {
            var nowUtc7 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VietnamTz);

            _collection.InsertOne(new PushPNSEMSLog
            {
                Request = request ?? "",
                Response = response ?? "",
                Time = nowUtc7,
                TimeText = nowUtc7.ToString("yyyy-MM-dd HH:mm:ss"),
                ItemCode = itemCode ?? "",
                Code = code ?? "",
                Success = success
            });

            _logger.LogInformation(
                "Đã ghi Mongo PushPNSEMS. ItemCode={ItemCode}, Success={Success}, Code={Code}, Time={Time}",
                itemCode,
                success,
                code,
                nowUtc7.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "PushPNSEMSMongoRepository.InsertLog failed. ItemCode={ItemCode}",
                itemCode);
        }
    }

    private static TimeZoneInfo ResolveVietnamTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
        }
    }
}
