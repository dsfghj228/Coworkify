using Backend.Models;

namespace Backend.Dto.WorkspaceDto;

public class ReturnWorkspace
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public List<Room> Rooms { get; set; }
}