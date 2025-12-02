using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CfxTestTool.Core.Messaging;

public class CfxReceiver : ICfxReceiver, IDisposable
{
    private readonly CfxSettings _settings;
    private IConnection? _connection;
    private IModel? _channel;
    private AsyncEventingBasicConsumer? _consumer;

    public event EventHandler<CfxReceivedMessageEventArgs>? MessageReceived;

    public CfxReceiver(CfxSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public Task StartAsync()
    {
        if (_connection != null)
        {
            return Task.CompletedTask;
        }

        var factory = new ConnectionFactory { Uri = new Uri(_settings.AmqpUri) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(_settings.Queue, durable: false, exclusive: false, autoDelete: false);

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += OnReceivedAsync;

        _channel.BasicConsume(queue: _settings.Queue, autoAck: true, consumer: _consumer);
        return Task.CompletedTask;
    }

    private Task OnReceivedAsync(object sender, BasicDeliverEventArgs e)
    {
        var payload = Encoding.UTF8.GetString(e.Body.ToArray());
        MessageReceived?.Invoke(this, new CfxReceivedMessageEventArgs(payload, e.RoutingKey));
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        if (_consumer != null)
        {
            _consumer.Received -= OnReceivedAsync;
        }

        _channel?.Close();
        _connection?.Close();

        _channel?.Dispose();
        _connection?.Dispose();

        _channel = null;
        _connection = null;
        _consumer = null;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        StopAsync().GetAwaiter().GetResult();
    }
}
