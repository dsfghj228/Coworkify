using System.Text;
using System.Text.Json;
using Backend.Interfaces;
using Backend.RabbitMq.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Backend.RabbitMq.Consumers;

public class RoomConsumer(
    ILogger<RoomConsumer> logger,
    IConfiguration config
) : BackgroundService
{
    private IConnection? _connection;
    private IModel? _channel;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMq:HostName"],
            Port = int.Parse(config["RabbitMq:Port"]),
            UserName = config["RabbitMq:UserName"],
            Password = config["RabbitMq:Password"],
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                logger.LogInformation($"Получено сообщение из RabbitMq: {json}");

                var data = JsonSerializer.Deserialize<WorkspaceEvent>(json);

                if (data != null && data.Type == "workspace.deleted")
                {
                    logger.LogInformation($"Успешное удаление комнат с Workspace Id: {data.WorkspaceId}");
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, "❌ Ошибка обработки сообщения из RabbitMQ");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: "workspace.events", autoAck: false, consumer: consumer);

        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
