using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZApiLogRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonIgnore]
        public string CollectionName { get; set; }
        public string TraceId { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }

        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public long DurationMs { get; set; }

        public string ExceptionMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
