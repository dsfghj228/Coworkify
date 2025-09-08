using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repository;

public class WorkspaceRepository(ApplicationDbContext context) : IWorkspaceRepository
{

    public async Task CreateWorkspace(Workspace workspace)
    {
       await context.Workspaces.AddAsync(workspace);
       await context.SaveChangesAsync();
    }
}