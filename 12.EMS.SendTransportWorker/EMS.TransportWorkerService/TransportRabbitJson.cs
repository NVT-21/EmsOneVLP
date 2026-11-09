using EMS.TransportWorkerService.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EMS.TransportWorkerService;

/// <summary>
/// JSON thống nhất cho message Rabbit (publisher + consumer).
/// Hỗ trợ cả PascalCase và camelCase vì API khác có thể publish vào cùng queue.
/// </summary>
public static class TransportRabbitJson
{
    private static readonly JsonSerializerSettings Pascal = new()
    {
        MissingMemberHandling = MissingMemberHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
    };

    private static readonly JsonSerializerSettings Camel = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        MissingMemberHandling = MissingMemberHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
    };

    public static string Serialize(object value) =>
        JsonConvert.SerializeObject(value, Camel);

    /// <summary>
    /// Thử deserialize với nhiều contract để <see cref="ZTransportRabbitMessage.QueueId"/> không bị null.
    /// </summary>
    public static ZTransportRabbitMessage? DeserializeTransportMessage(string json, ILogger? logger = null)
    {
        foreach (var settings in new[] { Pascal, Camel })
        {
            try
            {
                var m = JsonConvert.DeserializeObject<ZTransportRabbitMessage>(json, settings);
                if (m != null && !string.IsNullOrWhiteSpace(m.QueueId))
                    return m;
            }
            catch (JsonException ex)
            {
                logger?.LogDebug(ex, "Deserialize ZTransportRabbitMessage (một lần thử) thất bại");
            }
        }

        return null;
    }
}
