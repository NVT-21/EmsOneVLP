using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.TransportWorkerService.Models;
/// <summary>
/// Model mapping với collection MongoDB: transport_queue.
/// Collection này đóng vai trò Outbox lưu các yêu cầu gửi vận chuyển.
/// </summary>
[BsonIgnoreExtraElements]
public class ZTransportQueueMongo
{
    /// <summary>
    /// Khóa chính MongoDB.
    /// QueueId này sẽ được truyền sang RabbitMQ để Consumer xử lý tiếp.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Trạng thái amend của đơn hàng.
    /// Giữ nguyên theo request hiện tại của hệ thống.
    /// </summary>
    public string AmndState { get; set; }

    /// <summary>
    /// ID đơn hàng cần gửi vận chuyển.
    /// </summary>
    public int OrderID { get; set; }

    /// <summary>
    /// ID shop của đơn hàng.
    /// </summary>
    public int ShopID { get; set; }

    /// <summary>
    /// Người tạo yêu cầu, dùng để cập nhật UpdatedBy khi gửi vận chuyển thành công.
    /// Trong Mongo có thể là số hoặc chuỗi.
    /// </summary>
    public object CreatedBy { get; set; }

    /// <summary>
    /// Trạng thái xử lý:
    /// Pending: API vừa ghi vào MongoDB, chưa đẩy RabbitMQ.
    /// Queued: Publisher đã đẩy RabbitMQ.
    /// Processing: Consumer đang xử lý.
    /// Success: Gửi vận chuyển thành công.
    /// Failed: Gửi lỗi, còn có thể retry.
    /// DeadLetter: Lỗi quá số lần retry.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Số lần retry đã thực hiện.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Số lần retry tối đa trước khi chuyển DeadLetter.
    /// </summary>
    public int MaxRetry { get; set; } = 5;

    /// <summary>
    /// Lỗi cuối cùng nếu gửi vận chuyển thất bại.
    /// </summary>
    public string LastError { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? QueuedAt { get; set; }

    public DateTime? ProcessingAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID message trên Rabbit (nếu API ghi). Có thể null.
    /// </summary>
    public string? RabbitMessageId { get; set; }

    /// <summary>
    /// Ngày dạng số do API ghi (vd 20260514).
    /// </summary>
    public int? DateNumber { get; set; }
}