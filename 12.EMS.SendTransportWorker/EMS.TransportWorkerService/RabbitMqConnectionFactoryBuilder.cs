using System.Net.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EMS.TransportWorkerService;

/// <summary>
/// Tạo <see cref="ConnectionFactory"/> giống nhau cho consumer và publisher.
/// UI Management (15672) và AMQP (5672/5671) dùng chung user DB; nếu UI đăng nhập được mà AMQP ACCESS_REFUSED
/// thường gặp: cổng TLS (5671), User Secrets ghi đè mật khẩu, hoặc sai VirtualHost so với quyền thực tế.
/// </summary>
public static class RabbitMqConnectionFactoryBuilder
{
    public static ConnectionFactory Create(IConfiguration configuration, ILogger? logger = null)
    {
        var amqpUri = configuration["RabbitMQ:AmqpUri"]?.Trim();
        if (!string.IsNullOrEmpty(amqpUri))
        {
            try
            {
                var uriFactory = new ConnectionFactory
                {
                    ClientProvidedName = "EMS.TransportWorkerService",
                    Uri = new Uri(amqpUri)
                };
                logger?.LogInformation(
                    "RabbitMQ (AmqpUri): Host={Host}, Port={Port}, VirtualHost={VHost}, User={User}",
                    uriFactory.HostName,
                    uriFactory.Port,
                    uriFactory.VirtualHost,
                    uriFactory.UserName);
                return uriFactory;
            }
            catch (UriFormatException ex)
            {
                logger?.LogError(ex, "RabbitMQ:AmqpUri không hợp lệ, dùng HostName/UserName/Password.");
            }
        }

        var hostName = (configuration["RabbitMQ:HostName"] ?? "localhost").Trim();
        var userName = (configuration["RabbitMQ:UserName"] ?? "guest").Trim();
        var password = (configuration["RabbitMQ:Password"] ?? "guest").Trim();
        var virtualHost = configuration["RabbitMQ:VirtualHost"]?.Trim();
        if (string.IsNullOrEmpty(virtualHost))
            virtualHost = "/";

        var portConfigured = int.TryParse(configuration["RabbitMQ:Port"], out var parsedPort) && parsedPort > 0;
        var port = portConfigured ? parsedPort : 5672;

        var useSsl = configuration.GetValue("RabbitMQ:UseSsl", false);
        if (useSsl && !portConfigured)
            port = 5671;

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port,
            VirtualHost = virtualHost,
            UserName = userName,
            Password = password,
            ClientProvidedName = "EMS.TransportWorkerService",
            RequestedHeartbeat = TimeSpan.FromSeconds(60)
        };

        if (useSsl)
        {
            var sslServerName = (configuration["RabbitMQ:SslServerName"] ?? hostName).Trim();
            factory.Ssl = new SslOption
            {
                Enabled = true,
                ServerName = sslServerName,
                AcceptablePolicyErrors =
                    SslPolicyErrors.RemoteCertificateChainErrors |
                    SslPolicyErrors.RemoteCertificateNameMismatch
            };
        }

        logger?.LogInformation(
            "RabbitMQ: Host={Host}, Port={Port}, Ssl={Ssl}, VirtualHost={VHost}, User={User}, PasswordLength={PwdLen}",
            hostName,
            port,
            useSsl,
            virtualHost,
            userName,
            password.Length);

        return factory;
    }
}
