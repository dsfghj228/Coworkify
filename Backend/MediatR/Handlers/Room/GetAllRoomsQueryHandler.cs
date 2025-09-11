using Backend.Dto.RoomDto;
using Backend.Interfaces;
using Backend.MediatR.Queries.Room;
using MediatR;

namespace Backend.MediatR.Handlers.Room;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository)  : IRequestHandler<GetAllRoomsQuery, List<ReturnRoom>>
{
    public async Task<List<ReturnRoom>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(request.WorkspaceId);
        
        var roomsForReturn = new List<ReturnRoom>();
        foreach (var room in rooms)
        {
            var roomForReturn = new ReturnRoom
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HourlyRate = room.HourlyRate,
                WorkspaceId = room.WorkspaceId
            };
            roomsForReturn.Add(roomForReturn);
        }
        
        return roomsForReturn;
    }
}