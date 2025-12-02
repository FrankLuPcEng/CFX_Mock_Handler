using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using CfxTestTool.Core.Serialization;

namespace CfxTestTool.Core.Messaging;

public class CfxSender : ICfxSender
{
    private readonly CfxSettings _settings;
    private readonly JsonSerializerOptions _options = CfxJsonOptions.CreateDefault();

    public CfxSender(CfxSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task SendAsync(object message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var payload = JsonSerializer.Serialize(message, message.GetType(), _options);
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_settings.AmqpUri)
        };

        await Task.Run(() =>
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(payload);
            var properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";

            channel.BasicPublish(_settings.Exchange, _settings.RoutingKey, properties, body);
        }).ConfigureAwait(false);
    }
}
