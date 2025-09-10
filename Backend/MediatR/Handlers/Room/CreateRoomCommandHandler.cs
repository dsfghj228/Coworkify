using Backend.Dto.RoomDto;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Room;
using MediatR;

namespace Backend.MediatR.Handlers.Room;

public class CreateRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<CreateRoomCommand, ReturnRoom>
{
    public async Task<ReturnRoom> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.CreateRoom(request);
        var roomForReturn = new ReturnRoom
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyRate = room.HourlyRate,
            WorkspaceId = room.WorkspaceId,
            Bookings = room.Bookings
        };
        return roomForReturn;
    }
}