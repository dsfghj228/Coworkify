using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class WorkspaceRepository(ApplicationDbContext context) : IWorkspaceRepository
{

    public async Task CreateWorkspace(Workspace workspace)
    {
       await context.Workspaces.AddAsync(workspace);
       await context.SaveChangesAsync();
    }

    public async Task<List<Workspace>> GetAllWorkspaces()
    {
        return await context.Workspaces.Include(w => w.Rooms).ToListAsync();
    }
}