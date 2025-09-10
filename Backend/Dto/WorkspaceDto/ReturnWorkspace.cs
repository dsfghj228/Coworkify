using Backend.Dto.RoomDto;
using Backend.Models;

namespace Backend.Dto.WorkspaceDto;

public class ReturnWorkspace
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public List<ReturnRoom> Rooms { get; set; }
}