using Backend.Dto.RoomDto;
using MediatR;

namespace Backend.MediatR.Queries.Room;

public class GetRoomByIdQuery : IRequest<ReturnRoom>
{
    public Guid Id { get; set; }
}