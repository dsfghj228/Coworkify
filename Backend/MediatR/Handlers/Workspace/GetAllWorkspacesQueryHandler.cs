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
            var returnWorkspace = new ReturnWorkspace
            {
                Name = workspace.Name,
                Address = workspace.Address,
                Description = workspace.Description,
                Rooms = workspace.Rooms
            };
            returnWorkspaces.Add(returnWorkspace);
        }
        return returnWorkspaces;
    }
}