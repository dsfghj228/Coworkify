using Backend.Dto.WorkspaceDto;
using MediatR;

namespace Backend.MediatR.Queries.Workspace;

public class GetAllWorkspacesQuery : IRequest<IList<ReturnWorkspace>>;