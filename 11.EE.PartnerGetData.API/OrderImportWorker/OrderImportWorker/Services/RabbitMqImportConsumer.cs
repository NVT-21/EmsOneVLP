using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderImportWorker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Services
{
    public class RabbitMqImportConsumer
    {
        private readonly RabbitMqOptions _rabbitOptions;
        private readonly ImportJobProcessor _processor;
        private readonly ILogger<RabbitMqImportConsumer> _logger;

        private IConnection? _connection;
        private IChannel? _channel;
        private string? _consumerTag;

        public RabbitMqImportConsumer(
            IOptions<RabbitMqOptions> rabbitOptions,
            ImportJobProcessor processor,
            ILogger<RabbitMqImportConsumer> logger)
        {
            _rabbitOptions = rabbitOptions.Value;
            _processor = processor;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitOptions.Host,
                UserName = _rabbitOptions.UserName,
                Password = _rabbitOptions.Password
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.ExchangeDeclareAsync(
                exchange: _rabbitOptions.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _rabbitOptions.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await _channel.QueueBindAsync(
                queue: _rabbitOptions.QueueName,
                exchange: _rabbitOptions.ExchangeName,
                routingKey: _rabbitOptions.RoutingKey,
                cancellationToken: cancellationToken);

            await _channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var job = JsonConvert.DeserializeObject<OrderImportJobMessage>(json);

                    if( job == null )
                        throw new Exception("Job message null");

                    _logger.LogInformation("Received job {JobId}", job.JobId);

                    await _processor.Process(job);

                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: cancellationToken);

                    _logger.LogInformation("Ack job {JobId}", job.JobId);
                }
                catch( Exception ex )
                {
                    _logger.LogError(ex, "Process RabbitMQ message failed");

                    if( _channel != null && _channel.IsOpen )
                    {
                        await _channel.BasicNackAsync(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false,
                            requeue: false,
                            cancellationToken: cancellationToken);
                    }
                }
            };

            _consumerTag = await _channel.BasicConsumeAsync(
                queue: _rabbitOptions.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "RabbitMQ consumer started. Queue={QueueName}, ConsumerTag={ConsumerTag}",
                _rabbitOptions.QueueName,
                _consumerTag);
        }

        public async Task StopAsync()
        {
            try
            {
                if( _channel != null && _channel.IsOpen && !string.IsNullOrWhiteSpace(_consumerTag) )
                {
                    await _channel.BasicCancelAsync(_consumerTag);
                }

                if( _channel != null )
                {
                    await _channel.CloseAsync();
                    await _channel.DisposeAsync();
                }

                if( _connection != null )
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }
            }
            catch( Exception ex )
            {
                _logger.LogError(ex, "Error while stopping RabbitMQ consumer");
            }
        }
    }
}
