using EMS.TransportWorkerService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EMS.TransportWorkerService.Repositories;
/// <summary>
/// Repository thao tác với MongoDB transport_queue.
/// Project Publisher chỉ cần đọc Pending và chuyển thành Queued.
/// </summary>
public class TransportQueueMongoRepository
{
    private readonly IMongoCollection<ZTransportQueueMongo> _collection;

    public TransportQueueMongoRepository(IConfiguration configuration)
    {
        var connectionString = configuration["Mongo:ConnectionString"];
        var databaseName = configuration["Mongo:DatabaseName"];
        var collectionName = configuration["Mongo:CollectionName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _collection = database.GetCollection<ZTransportQueueMongo>(collectionName);
    }

    /// <summary>
    /// Lấy một bản ghi Pending để đẩy sang RabbitMQ.
    /// Dùng FindOneAndUpdate để tránh nhiều Publisher cùng lấy trùng một bản ghi.
    /// 
    /// Logic:
    /// - Chỉ lấy Status = Pending.
    /// - Chỉ lấy bản ghi chưa quá số lần retry.
    /// - Khi lấy được thì đổi ngay sang Queued.
    /// </summary>
    public ZTransportQueueMongo GetOnePendingForQueue()
    {
        FilterDefinition<ZTransportQueueMongo> filter = new BsonDocument
        {
            { "Status", "Pending" },
            {
                "$expr", new BsonDocument("$lt", new BsonArray
                {
                    "$RetryCount",
                    "$MaxRetry"
                })
            }
        };

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, "Queued")
            .Set(x => x.QueuedAt, DateTime.Now)
            .Set(x => x.UpdatedAt, DateTime.Now);

        var options = new FindOneAndUpdateOptions<ZTransportQueueMongo>
        {
            ReturnDocument = ReturnDocument.After
        };

        return _collection.FindOneAndUpdate(filter, update, options);
    }

    /// <summary>
    /// Debug: lấy đúng OrderID + ShopID bất kể Status, reset RetryCount và chuyển Queued để test lại.
    /// </summary>
    public ZTransportQueueMongo GetOneForQueueByOrderShop(int orderId, int shopId)
    {
        var filter = Builders<ZTransportQueueMongo>.Filter.And(
            Builders<ZTransportQueueMongo>.Filter.Eq(x => x.OrderID, orderId),
            Builders<ZTransportQueueMongo>.Filter.Eq(x => x.ShopID, shopId));

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, "Queued")
            .Set(x => x.RetryCount, 0)
            .Set(x => x.LastError, null)
            .Set(x => x.QueuedAt, DateTime.Now)
            .Set(x => x.UpdatedAt, DateTime.Now);

        var options = new FindOneAndUpdateOptions<ZTransportQueueMongo>
        {
            Sort = Builders<ZTransportQueueMongo>.Sort.Combine(
                Builders<ZTransportQueueMongo>.Sort.Descending(x => x.UpdatedAt),
                Builders<ZTransportQueueMongo>.Sort.Descending(x => x.CreatedAt)),
            ReturnDocument = ReturnDocument.After
        };

        return _collection.FindOneAndUpdate(filter, update, options);
    }

    /// <summary>
    /// Nếu publish RabbitMQ thất bại thì đưa bản ghi quay lại Pending
    /// để lần chạy sau Publisher đẩy lại.
    /// </summary>
    public void RevertToPending(string queueId, string error)
    {
        var objectId = MongoDB.Bson.ObjectId.Parse(queueId);

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, "Pending")
            .Set(x => x.LastError, error)
            .Set(x => x.UpdatedAt, DateTime.Now);

        _collection.UpdateOne(x => x.Id == objectId, update);
    }

    public void MarkProcessing(string id)
    {
        var objectId = MongoDB.Bson.ObjectId.Parse(id);

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, "Processing")
            .Set(x => x.ProcessingAt, DateTime.Now)
            .Set(x => x.UpdatedAt, DateTime.Now);

        _collection.UpdateOne(x => x.Id == objectId, update);
    }

    public void MarkSuccess(string id)
    {
        var objectId = MongoDB.Bson.ObjectId.Parse(id);

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, "Success")
            .Set(x => x.LastError, null)
            .Set(x => x.FinishedAt, DateTime.Now)
            .Set(x => x.UpdatedAt, DateTime.Now);

        _collection.UpdateOne(x => x.Id == objectId, update);
    }

    public void MarkFailed(string id, string error)
    {
        var objectId = MongoDB.Bson.ObjectId.Parse(id);

        var item = _collection.Find(x => x.Id == objectId).FirstOrDefault();

        if (item == null)
            return;

        var retryCount = item.RetryCount + 1;
        var newStatus = retryCount >= item.MaxRetry ? "DeadLetter" : "Pending";

        var update = Builders<ZTransportQueueMongo>.Update
            .Set(x => x.Status, newStatus)
            .Set(x => x.RetryCount, retryCount)
            .Set(x => x.LastError, error)
            .Set(x => x.UpdatedAt, DateTime.Now);

        _collection.UpdateOne(x => x.Id == objectId, update);
    }
}