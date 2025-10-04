using System.Text;
using System.Text.Json;
using Backend.Interfaces;
using RabbitMQ.Client;

namespace Backend.RabbitMq.Producers;

public class WorkspaceProducer(ILogger<WorkspaceProducer> logger, IConfiguration config) : IWorkspaceProducer
{
    public void WorkspaceDeleteProducer(object workspaceEvent)
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
        
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(workspaceEvent));
        channel.BasicPublish(
            exchange: "coworkify.events",
            routingKey: "workspace.deleted",
            basicProperties: null,
            body: body
        );
        logger.LogInformation("Сообщение об удалении рабочего пространства успешно отправлено!");
    }
}