using Backend.Dto.RoomDto;
using MediatR;

namespace Backend.MediatR.Commands.Room;

public class CreateRoomCommand : IRequest<ReturnRoom>
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public double HourlyRate { get; set; }
    public Guid WorkspaceId { get; set; }
}