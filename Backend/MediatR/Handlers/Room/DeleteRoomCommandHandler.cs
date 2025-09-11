using Backend.Dto.RoomDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Room;
using MediatR;

namespace Backend.MediatR.Handlers.Room;

public class DeleteRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<DeleteRoomCommand, ReturnRoom>
{
    public async Task<ReturnRoom> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.DeleteRoom(request.Id);

        var roomFotReturn = new ReturnRoom
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyRate = room.HourlyRate,
            WorkspaceId = room.WorkspaceId
        };
        return roomFotReturn;
    }
}