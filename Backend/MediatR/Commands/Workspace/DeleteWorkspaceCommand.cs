using Backend.Dto.WorkspaceDto;
using MediatR;

namespace Backend.MediatR.Commands.Workspace;

public class DeleteWorkspaceCommand : IRequest<ReturnWorkspace>
{
    public Guid Id { get; set; }
}