namespace Backend.RabbitMq.Events;

public class WorkspaceEvent
{
    public Guid EventId { get; set; }
    public string Type { get; set; }
    public Guid WorkspaceId { get; set; }
    public DateTime OccurredAt { get; set; }
}