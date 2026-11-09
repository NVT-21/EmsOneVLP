using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.TransportWorkerService.Models;

/// <summary>
/// Log đẩy PNS EMS → collection Mongo PushPNSEMS.
/// </summary>
[BsonIgnoreExtraElements]
public class PushPNSEMSLog
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Request { get; set; }
    public string Response { get; set; }
    public DateTime Time { get; set; }
    public string TimeText { get; set; }
    public string ItemCode { get; set; }
    public string Code { get; set; }
    public bool Success { get; set; }
}
