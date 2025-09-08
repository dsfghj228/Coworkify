namespace Backend.Models;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public double HourlyRate { get; set; }
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; }
    public List<Booking> Bookings { get; set; }
}