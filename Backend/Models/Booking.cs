namespace Backend.Models;

public class Booking
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public decimal TotalPrice { get; set; }
}