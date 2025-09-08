using Backend.Models;

namespace Backend.Interfaces;

public interface IWorkspaceRepository
{
    Task CreateWorkspace(Workspace workspace);
    Task<List<Workspace>> GetAllWorkspaces();
}