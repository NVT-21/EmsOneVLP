using EMS.Core;
using EMS.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EMS.API.Core.MongoRepository
{
    public class TransportMongoRepository
    {
        private readonly IMongoCollection<ZTransportQueueMongo> _collection;

        public TransportMongoRepository()
        {

            string connectionString = ConfigurationManager.AppSettings["MONGO_LOG_CONNECTION"];
            string databaseName = ConfigurationManager.AppSettings["MONGO_LOG_DATABASE"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _collection = database.GetCollection<ZTransportQueueMongo>("OrderSendTransport");
        }

        public ZTransportQueueMongo InsertPending(ZOrderSendTransportRequest req)
        {
            var item = new ZTransportQueueMongo
            {
                AmndState = req.AmndState,
                OrderID = req.OrderID,
                ShopID = req.ShopID,
                CreatedBy = req.CreatedBy,
                Status = "Pending",
                RetryCount = 0,
                MaxRetry = 5,
                DateNumber = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _collection.InsertOne(item);

            return item;
        }

        //public ZTransportQueueMongo GetOnePendingForQueue()
        //{
        //    var filter = Builders<ZTransportQueueMongo>.Filter.And(
        //        Builders<ZTransportQueueMongo>.Filter.Eq(x => x.Status, "Pending"),
        //        Builders<ZTransportQueueMongo>.Filter.Lt(x => x.RetryCount, x.MaxRetry)
        //    );

        //    var update = Builders<ZTransportQueueMongo>.Update
        //        .Set(x => x.Status, "Queued")
        //        .Set(x => x.QueuedAt, DateTime.Now)
        //        .Set(x => x.UpdatedAt, DateTime.Now)
        //        .Set(x => x.RabbitMessageId, Guid.NewGuid().ToString("N"));

        //    var options = new FindOneAndUpdateOptions<ZTransportQueueMongo>
        //    {
        //        ReturnDocument = ReturnDocument.After
        //    };

        //    return _collection.FindOneAndUpdate(filter, update, options);
        //}

        public ZTransportQueueMongo GetById(string id)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);

            return _collection
                .Find(x => x.Id == objectId)
                .FirstOrDefault();
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

            if(item == null)
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
}