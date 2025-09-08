namespace Backend.Models;

public class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public List<Room> Rooms { get; set; }
}