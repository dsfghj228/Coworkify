using Backend.Dto.RoomDto;
using Backend.Interfaces;
using Backend.MediatR.Queries.Room;
using MediatR;

namespace Backend.MediatR.Handlers.Room;

public class GetRoomByIdQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomByIdQuery, ReturnRoom>
{
    public async Task<ReturnRoom> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.Id);

        var roomForReturn = new ReturnRoom
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyRate = room.HourlyRate,
            WorkspaceId = room.WorkspaceId
        };
        
        return roomForReturn;
    }
}