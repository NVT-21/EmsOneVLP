using EMS.TransportWorkerService.Models;
using EMS.TransportWorkerService.Repositories;
using Microsoft.Extensions.Logging;

namespace EMS.TransportWorkerService.Workers;

/// <summary>
/// Đọc bản ghi <see cref="ZTransportQueueMongo"/> trạng thái Pending từ MongoDB,
/// chuyển sang Queued (atomic trong repository), publish body sang RabbitMQ.
/// Consumer (<see cref="RabbitTransportConsumerWorker"/>) nhận message và xử lý gửi vận chuyển.
/// </summary>
public class TransportMongoPublisherWorker : BackgroundService
{
    private readonly ILogger<TransportMongoPublisherWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly TransportQueueMongoRepository _mongoRepo;
    private readonly TransportRabbitPublisher _rabbitPublisher;
    private bool _debugForceOrderDone;

    public TransportMongoPublisherWorker(
        ILogger<TransportMongoPublisherWorker> logger,
        IConfiguration configuration,
        TransportQueueMongoRepository mongoRepo,
        TransportRabbitPublisher rabbitPublisher)
    {
        _logger = logger;
        _configuration = configuration;
        _mongoRepo = mongoRepo;
        _rabbitPublisher = rabbitPublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var debugForce = _configuration.GetValue("Worker:DebugForceOrder:Enabled", false);
        var debugOrderId = _configuration.GetValue("Worker:DebugForceOrder:OrderID", 0);
        var debugShopId = _configuration.GetValue("Worker:DebugForceOrder:ShopID", 0);

        _logger.LogInformation(
            "Mongo → Rabbit publisher đã chạy (lấy Pending, publish queue transport). DebugForceOrder={Enabled}, OrderID={OrderID}, ShopID={ShopID}",
            debugForce,
            debugOrderId,
            debugShopId);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ZTransportQueueMongo doc = null;

                // Chỉ force 1 lần khi start debug, tránh đẩy lặp vô hạn cùng đơn.
                if (debugForce && !_debugForceOrderDone && debugOrderId > 0 && debugShopId > 0)
                {
                    doc = _mongoRepo.GetOneForQueueByOrderShop(debugOrderId, debugShopId);
                    _debugForceOrderDone = true;

                    if (doc == null)
                    {
                        _logger.LogWarning(
                            "DebugForceOrder: không tìm thấy Mongo OrderID={OrderID}, ShopID={ShopID} (bất kỳ Status).",
                            debugOrderId,
                            debugShopId);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "DebugForceOrder: đã lấy QueueId={QueueId}, OrderID={OrderID}, ShopID={ShopID}, Status(trước→Queued).",
                            doc.Id,
                            doc.OrderID,
                            doc.ShopID);
                    }
                }
                else
                {
                    doc = _mongoRepo.GetOnePendingForQueue();
                }

                if (doc == null)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(750), stoppingToken);
                    continue;
                }

                var message = new ZTransportRabbitMessage
                {
                    QueueId = doc.Id.ToString(),
                    AmndState = doc.AmndState ?? string.Empty,
                    OrderID = doc.OrderID,
                    ShopID = doc.ShopID,
                    CreatedBy = FormatCreatedBy(doc.CreatedBy)
                };

                try
                {
                    _logger.LogInformation(
                        "Đang publish Rabbit (trước BasicPublish). QueueId={QueueId}, OrderID={OrderID}, ShopID={ShopID}",
                        message.QueueId,
                        message.OrderID,
                        message.ShopID);

                    await _rabbitPublisher.PublishAsync(message);
                    _logger.LogInformation(
                        "Đã publish Rabbit xong (broker đã nhận). QueueId={QueueId}, OrderID={OrderID}, ShopID={ShopID}",
                        message.QueueId,
                        message.OrderID,
                        message.ShopID);
                }
                catch (Exception ex)
                {
                    _mongoRepo.RevertToPending(message.QueueId, ex.Message);
                    _logger.LogWarning(ex, "Publish Rabbit thất bại, đã RevertToPending. QueueId={QueueId}", message.QueueId);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi vòng lặp Mongo publisher: {Error}", ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }

    private static string FormatCreatedBy(object? value) =>
        value switch
        {
            null => "0",
            string s => s,
            int i => i.ToString(),
            long l => l.ToString(),
            _ => value.ToString() ?? "0"
        };
}
