namespace Backend.Dto.RoomDto;

public class CreateRoom
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public double HourlyRate { get; set; }
    public Guid WorkspaceId { get; set; }
}