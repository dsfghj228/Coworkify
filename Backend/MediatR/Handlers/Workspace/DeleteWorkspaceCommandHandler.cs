using Backend.Dto.RoomDto;
using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Workspace;
using MediatR;

namespace Backend.MediatR.Handlers.Workspace;

public class DeleteWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository) : IRequestHandler<DeleteWorkspaceCommand, ReturnWorkspace>
{
    public async Task<ReturnWorkspace> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await workspaceRepository.DeleteWorkspace(request.Id);
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
            Description = workspace.Description,
            Name = workspace.Name,
            Rooms = roomsForReturn
        };
        return workspaceForReturn;
    }
}