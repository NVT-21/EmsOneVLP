using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.TransportWorkerService.Models;

/// <summary>
/// Log đẩy PNS VNPOST thành công → collection Mongo PushPNSVNPOST.
/// </summary>
[BsonIgnoreExtraElements]
public class PushPNSVNPOSTLog
{
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>JSON request gửi sang Partner/EMS/CreateOrder.</summary>
    public string Request { get; set; }

    /// <summary>Raw response từ PNS VNPOST.</summary>
    public string Response { get; set; }

    /// <summary>Thời điểm ghi log (UTC+7).</summary>
    public DateTime Time { get; set; }

    /// <summary>Chuỗi thời gian UTC+7 dễ đọc.</summary>
    public string TimeText { get; set; }

    public string ItemCode { get; set; }

    /// <summary>Mã phản hồi parse được (00/01/...).</summary>
    public string Code { get; set; }

    /// <summary>true khi Code = 00 hoặc 01.</summary>
    public bool Success { get; set; }
}
