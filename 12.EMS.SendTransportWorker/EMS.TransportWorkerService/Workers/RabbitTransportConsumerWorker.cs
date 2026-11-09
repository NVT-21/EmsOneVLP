using System.Text;
using EMS.TransportWorkerService;
using EMS.TransportWorkerService.Models;
using EMS.TransportWorkerService.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EMS.TransportWorkerService.Workers;

/// <summary>
/// Worker đọc RabbitMQ và gửi vận chuyển thật.
/// Đây là project thay thế phần gọi trực tiếp trong API cũ.
/// </summary>
public class RabbitTransportConsumerWorker : BackgroundService
{
    private readonly ILogger<RabbitTransportConsumerWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly TransportQueueMongoRepository _mongoRepo;
    private readonly EMSRepository _emsRepository;
    private readonly OrderRepository _orderRepository;
    private readonly PNSVnpostRepository _pnsVnpostRepository;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitTransportConsumerWorker(
        ILogger<RabbitTransportConsumerWorker> logger,
        IConfiguration configuration,
        TransportQueueMongoRepository mongoRepo,
        EMSRepository emsRepository,
        OrderRepository orderRepository,
        PNSVnpostRepository pnsVnpostRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _mongoRepo = mongoRepo;
        _emsRepository = emsRepository;
        _orderRepository = orderRepository;
        _pnsVnpostRepository = pnsVnpostRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunRabbitConsumerSessionAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Phiên Rabbit consumer kết thúc lỗi; thử lại sau 3 giây.");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }

    private async Task RunRabbitConsumerSessionAsync(CancellationToken stoppingToken)
    {
        try
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        catch
        {
            // ignore
        }

        _channel = null;
        _connection = null;

        var factory = RabbitMqConnectionFactoryBuilder.Create(_configuration, _logger);
        var queueName = _configuration["RabbitMQ:QueueName"]?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(queueName))
            throw new InvalidOperationException("Thiếu cấu hình RabbitMQ:QueueName.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                break;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                var detail = ex.InnerException?.Message ?? ex.Message;
                _logger.LogWarning(
                    ex,
                    "Chưa kết nối được RabbitMQ. Chi tiết: {Detail}. Đang dùng: {Host}:{Port}, Ssl={Ssl}, vhost={VHost}, user={User}.",
                    detail,
                    factory.HostName,
                    factory.Port,
                    factory.Ssl?.Enabled ?? false,
                    factory.VirtualHost,
                    factory.UserName);
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        if (_connection == null)
            return;

        // BasicConsumeAsync chỉ hoàn tất đăng ký consumer, không chờ đến khi đóng kênh.
        // Nếu không chờ ChannelShutdown, RunRabbitConsumerSessionAsync thoát sớm → ExecuteAsync gọi lại
        // và dispose connection trong khi ReceivedAsync vẫn chạy → BasicAckAsync: "Already closed".
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        var channel = _channel!;

        var channelClosed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        AsyncEventHandler<ShutdownEventArgs>? onChannelShutdown = null;
        onChannelShutdown = async (_, args) =>
        {
            _logger.LogInformation(
                "Rabbit channel shutdown. ReplyCode={ReplyCode} ReplyText={ReplyText}",
                args.ReplyCode,
                args.ReplyText);
            channelClosed.TrySetResult();
            await Task.CompletedTask.ConfigureAwait(false);
        };

        channel.ChannelShutdownAsync += onChannelShutdown;
        try
        {
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken
            );

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false,
                cancellationToken: stoppingToken
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                await Task.Yield();

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                ZTransportRabbitMessage? message = null;

                try
                {
                    _logger.LogInformation(
                        "Rabbit nhận message: queue={Queue}, bytes={Bytes}",
                        queueName,
                        ea.Body.Length);

                    message = TransportRabbitJson.DeserializeTransportMessage(json, _logger);

                    if (message == null || string.IsNullOrWhiteSpace(message.QueueId))
                    {
                        var snippet = json.Length > 400 ? json[..400] + "..." : json;
                        _logger.LogWarning(
                            "Bỏ qua message không đọc được ZTransportRabbitMessage (QueueId trống). Snippet={Snippet}",
                            snippet);
                        await TryBasicAckAsync(channel, ea.DeliveryTag, stoppingToken).ConfigureAwait(false);
                        return;
                    }

                    _logger.LogInformation(
                        "Received transport message. QueueId={QueueId}, OrderID={OrderID}, ShopID={ShopID}",
                        message.QueueId,
                        message.OrderID,
                        message.ShopID);

                    _mongoRepo.MarkProcessing(message.QueueId);

                    _logger.LogInformation(
                        "Mongo MarkProcessing xong. QueueId={QueueId}, OrderID={OrderID} — tiếp theo Oracle SendTransportInfoVNPOST rồi PNS Vnpost + PNS EMS song song.",
                        message.QueueId,
                        message.OrderID);

                    var maxSec = _configuration.GetValue("Worker:MaxProcessingSecondsPerMessage", 300);
                    if (maxSec < 30)
                        maxSec = 30;

                    using var processingCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    processingCts.CancelAfter(TimeSpan.FromSeconds(maxSec));

                    ZResponse resSendRequestTransport;
                    try
                    {
                        resSendRequestTransport = await XCommon.SendTransportOrder(
                            message.AmndState,
                            message.OrderID,
                            message.ShopID,
                            _logger,
                            _emsRepository,
                            _configuration,
                            _orderRepository,
                            _pnsVnpostRepository,
                            processingCts.Token
                        ).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException ex) when (!stoppingToken.IsCancellationRequested)
                    {
                        _mongoRepo.MarkFailed(
                            message.QueueId,
                            $"Hết thời gian xử lý tối đa {maxSec}s (Worker:MaxProcessingSecondsPerMessage). {ex.Message}");
                        _logger.LogWarning(
                            ex,
                            "Timeout xử lý message Rabbit. QueueId={QueueId}, OrderID={OrderID}",
                            message.QueueId,
                            message.OrderID);
                        await TryBasicAckAsync(channel, ea.DeliveryTag, stoppingToken).ConfigureAwait(false);
                        return;
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation(
                            "Host đang shutdown, dừng xử lý message. QueueId={QueueId}",
                            message.QueueId);
                        return;
                    }

                    ZResponse resUpdateTransport;

                    if (resSendRequestTransport.Code == "00" || resSendRequestTransport.Code == "01")
                    {
                        resUpdateTransport = _orderRepository.SendTransportSuccess(
                            new ZOrderSendTransportSuccessRequest
                            {
                                AmndState = message.AmndState,
                                OrderID = message.OrderID,
                                ShopID = message.ShopID,
                                UpdatedBy = int.TryParse(message.CreatedBy, out var createdBy) ? createdBy : 0
                            },
                            _logger);
                    }
                    else
                    {
                        resUpdateTransport = _orderRepository.SendTransportError(
                            new ZOrderSendTransportErrorRequest
                            {
                                AmndState = message.AmndState,
                                OrderID = message.OrderID,
                                ShopID = message.ShopID
                            },
                            _logger);
                    }

                    // Mongo Success chỉ khi EMS/Oracle SEND_TRANSPORT thành công (SendCode=00).
                    // SendTransportError cũng trả Code "00" khi *ghi lỗi* vào Oracle thành công — không được nhầm với gửi vận chuyển OK.
                    if ((resSendRequestTransport.Code == "00" && resUpdateTransport.Code == "00") || (resSendRequestTransport.Code == "01" && resUpdateTransport.Code == "00"))
                    {
                        _mongoRepo.MarkSuccess(message.QueueId);

                        _logger.LogInformation(
                            "Transport completed. QueueId={QueueId}, OrderID={OrderID}, SendCode={SendCode}",
                            message.QueueId,
                            message.OrderID,
                            resSendRequestTransport.Code);
                    }
                    else
                    {
                        var failurePayload = new
                        {
                            Send = resSendRequestTransport,
                            OracleUpdate = resUpdateTransport
                        };
                        var failureJson = JsonConvert.SerializeObject(failurePayload);

                        _mongoRepo.MarkFailed(message.QueueId, failureJson);

                        _logger.LogWarning(
                            "Transport failed or Oracle update failed. QueueId={QueueId}, OrderID={OrderID}, SendCode={SendCode}, Response={Response}",
                            message.QueueId,
                            message.OrderID,
                            resSendRequestTransport.Code,
                            failureJson);
                    }

                    await TryBasicAckAsync(channel, ea.DeliveryTag, stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (message != null && !string.IsNullOrEmpty(message.QueueId))
                    {
                        _mongoRepo.MarkFailed(message.QueueId, ex.Message);
                    }

                    _logger.LogError(ex, "Rabbit consumer error. RawMessage={Message}", json);

                    await TryBasicAckAsync(channel, ea.DeliveryTag, stoppingToken).ConfigureAwait(false);
                }
            };

            _logger.LogInformation(
                "Rabbit consumer đăng ký BasicConsume, queue={Queue} (chờ message)...",
                queueName);

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            ).ConfigureAwait(false);

            _logger.LogInformation(
                "Rabbit consumer đang chờ (giữ phiên đến khi channel đóng hoặc host dừng). queue={Queue}",
                queueName);

            try
            {
                await channelClosed.Task.WaitAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await channel.CloseAsync(
                        replyCode: 200,
                        replyText: "Application shutting down",
                        abort: false,
                        cancellationToken: stoppingToken).ConfigureAwait(false);
                }
                catch (Exception closeEx)
                {
                    _logger.LogDebug(closeEx, "CloseAsync kênh khi dừng host (có thể bỏ qua).");
                }
            }

            _logger.LogInformation("EMS Transport Consumer Worker session kết thúc.");
        }
        finally
        {
            if (onChannelShutdown != null)
                channel.ChannelShutdownAsync -= onChannelShutdown;
        }
    }

    /// <summary>
    /// Ack an toàn khi connection/channel có thể đã đóng (đóng do phiên mới hoặc shutdown).
    /// </summary>
    private static async Task TryBasicAckAsync(IChannel channel, ulong deliveryTag, CancellationToken cancellationToken)
    {
        try
        {
            await channel.BasicAckAsync(deliveryTag, false, cancellationToken).ConfigureAwait(false);
        }
        catch (AlreadyClosedException)
        {
            // Channel đã đóng — broker có thể đã redeliver; log ở mức debug.
        }
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex) when (ex.Message.Contains("Already closed", StringComparison.OrdinalIgnoreCase))
        {
        }
    }

    public override void Dispose()
    {
        try
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        catch
        {
            // Ignore dispose exception.
        }

        base.Dispose();
    }
}
