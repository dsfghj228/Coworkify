using Backend.Dto.WorkspaceDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Workspace;
using Backend.Models;
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
            Rooms = new List<Room>()
        };
        
        await workspaceRepository.CreateWorkspace(workspace);

        var workspaceForReturn = new ReturnWorkspace
        {
            Id = workspace.Id,
            Address = workspace.Address,
            Name = workspace.Name,
            Description = workspace.Description,
            Rooms = workspace.Rooms
        };
        
        return workspaceForReturn;
    }
}