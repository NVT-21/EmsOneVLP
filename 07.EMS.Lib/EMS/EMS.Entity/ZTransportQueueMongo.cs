using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportQueueMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string AmndState { get; set; }

        public int OrderID { get; set; }

        public int ShopID { get; set; }

        public int CreatedBy { get; set; }

        public string Status { get; set; }
        // Pending, Queued, Processing, Success, Failed, DeadLetter

        public int RetryCount { get; set; }

        public int MaxRetry { get; set; } = 5;

        public string LastError { get; set; }

        public string RabbitMessageId { get; set; }
        public int DateNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? QueuedAt { get; set; }

        public DateTime? ProcessingAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
