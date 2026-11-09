namespace EMS.TransportWorkerService.Models;
/// <summary>
/// Message được Publisher đẩy vào RabbitMQ.
/// Consumer sẽ đọc message này để gọi gửi vận chuyển thật.
/// </summary>
public class ZTransportRabbitMessage
{
    /// <summary>
    /// Chính là _id trong MongoDB.
    /// Dùng để Consumer cập nhật lại trạng thái xử lý.
    /// </summary>
    public string QueueId { get; set; }

    public string AmndState { get; set; }

    public int OrderID { get; set; }

    public int ShopID { get; set; }

    public string CreatedBy { get; set; }
}