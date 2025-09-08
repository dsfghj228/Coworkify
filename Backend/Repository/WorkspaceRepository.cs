using Backend.Data;
using Backend.Exceptions;
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

    public async Task<Workspace> GetWorkspaceById(Guid id)
    {
        var workspaces = await context.Workspaces.Include(w => w.Rooms).FirstOrDefaultAsync(w => w.Id == id);
        if (workspaces == null)
        {
            throw new CustomExceptions.WorkspaceNotFoundException(id);
        }
        return workspaces;
    }
}