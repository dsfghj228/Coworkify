using Backend.Dto.RoomDto;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Queries.Workspace;
using MediatR;

namespace Backend.MediatR.Handlers.Workspace;

public class GetAllWorkspacesQueryHandler(IWorkspaceRepository workspaceRepository) : IRequestHandler<GetAllWorkspacesQuery, IList<ReturnWorkspace>>
{
    public async Task<IList<ReturnWorkspace>> Handle(GetAllWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await workspaceRepository.GetAllWorkspaces();
        var returnWorkspaces = new List<ReturnWorkspace>();
        foreach (var workspace in workspaces)
        {
            var roomsForReturn = new List<ReturnRoom>();

            foreach (var room in workspace.Rooms)
            {
                var roomForReturn = new ReturnRoom
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    HourlyRate = room.HourlyRate,
                    Bookings = room.Bookings,
                    WorkspaceId = room.WorkspaceId
                };
                roomsForReturn.Add(roomForReturn);
            }
            
            var returnWorkspace = new ReturnWorkspace
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Address = workspace.Address,
                Description = workspace.Description,
                Rooms = roomsForReturn
            };
            returnWorkspaces.Add(returnWorkspace);
        }
        return returnWorkspaces;
    }
}