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
        var workspaceForReturn = new ReturnWorkspace
        {
            Id = workspace.Id,
            Address = workspace.Address,
            Description = workspace.Description,
            Name = workspace.Name,
            Rooms = workspace.Rooms
        };
        return workspaceForReturn;
    }
}