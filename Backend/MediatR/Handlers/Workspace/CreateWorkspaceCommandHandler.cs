using Backend.Dto.RoomDto;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Workspace;
using MediatR;

namespace Backend.MediatR.Handlers.Workspace;

public class CreateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository) : IRequestHandler<CreateWorkspaceCommand, ReturnWorkspace>
{
    public async Task<ReturnWorkspace> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = new Models.Workspace
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            OwnerId = request.Owner.Id,
            Owner = request.Owner,
            Rooms = new List<Models.Room>()
        };
        
        await workspaceRepository.CreateWorkspace(workspace);
        
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

        var workspaceForReturn = new ReturnWorkspace
        {
            Id = workspace.Id,
            Address = workspace.Address,
            Name = workspace.Name,
            Description = workspace.Description,
            Rooms = roomsForReturn
        };
        
        return workspaceForReturn;
    }
}