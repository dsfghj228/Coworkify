using Backend.Dto.RoomDto;
using MediatR;

namespace Backend.MediatR.Commands.Room;

public class DeleteRoomCommand  : IRequest<ReturnRoom>
{
    public Guid Id { get; set; }
}