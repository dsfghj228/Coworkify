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
        var workspaceForReturn = new ReturnWorkspace
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            Address = workspace.Address,
            Rooms = workspace.Rooms
        };
        return workspaceForReturn;
    }
}