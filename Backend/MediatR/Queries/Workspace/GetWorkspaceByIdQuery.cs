using Backend.Dto.WorkspaceDto;
using MediatR;

namespace Backend.MediatR.Queries.Workspace;

public class GetWorkspaceByIdQuery : IRequest<ReturnWorkspace>
{
    public Guid Id { get; set; }
}