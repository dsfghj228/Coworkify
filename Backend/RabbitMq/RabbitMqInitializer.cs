using RabbitMQ.Client;

namespace Backend.RabbitMq;

public class RabbitMqInitializer(IConfiguration config)
{
    public void Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMq:HostName"],
            Port = int.Parse(config["RabbitMq:Port"]),
            UserName = config["RabbitMq:UserName"],
            Password = config["RabbitMq:Password"],
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: "coworkify.events",
            type: ExchangeType.Topic,
            durable: true
            );
        
        channel.QueueDeclare(
            queue: "workspace.events",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        channel.QueueBind("workspace.events", "coworkify.events", "workspace.*");
    }
}