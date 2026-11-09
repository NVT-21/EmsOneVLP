using System.Text;
using EMS.TransportWorkerService;
using EMS.TransportWorkerService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EMS.TransportWorkerService.Repositories;

/// <summary>
/// Service chịu trách nhiệm publish message vào RabbitMQ.
/// </summary>
public class TransportRabbitPublisher
{
    private readonly IConfiguration _configuration;

    public TransportRabbitPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync(ZTransportRabbitMessage message)
    {
        var factory = RabbitMqConnectionFactoryBuilder.Create(_configuration);
        var queueName = _configuration["RabbitMQ:QueueName"]?.Trim() ?? "";

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = TransportRabbitJson.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = message.QueueId
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }
}