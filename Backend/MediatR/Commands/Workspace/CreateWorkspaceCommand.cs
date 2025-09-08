using Backend.Dto.WorkspaceDto;
using Backend.Models;
using MediatR;

namespace Backend.MediatR.Commands.Workspace;

public class CreateWorkspaceCommand : IRequest<ReturnWorkspace>
{
    public AppUser Owner { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
}