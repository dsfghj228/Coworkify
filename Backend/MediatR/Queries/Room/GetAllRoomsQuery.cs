using Backend.Dto.RoomDto;
using MediatR;

namespace Backend.MediatR.Queries.Room;

public class GetAllRoomsQuery : IRequest<List<ReturnRoom>>
{
    public Guid WorkspaceId { get; set; }
}