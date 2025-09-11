using Backend.Dto.RoomDto;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Queries.Workspace;
using MediatR;

namespace Backend.MediatR.Handlers.Workspace;

public class GetWorkspaceByIdQueryHandler(IWorkspaceRepository workspaceRepository)  : IRequestHandler<GetWorkspaceByIdQuery, ReturnWorkspace>
{
    public async Task<ReturnWorkspace> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.GetWorkspaceById(request.Id);
        
        var roomsForReturn = new List<ReturnRoom>();

        foreach (var room in workspace.Rooms)
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
        
        var workspaceForReturn = new ReturnWorkspace
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            Address = workspace.Address,
            Rooms = roomsForReturn
        };
        return workspaceForReturn;
    }
}