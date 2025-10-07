using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.RabbitMq.Events;
using Backend.RabbitMq.Producers;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.UnitTests;

[TestFixture]
public class WorkspaceRepositoryTests()
{
    private DbContextOptions<ApplicationDbContext> _dbOptions;
    private static readonly Guid WorkspaceId1 = Guid.Parse("fb0cd5d2-1385-4408-9677-a8e0058b0240");
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("WorkspaceTestsDB")
            .Options;
        using var context = new ApplicationDbContext(_dbOptions);
        
        var workspace1 = new Workspace
        {
            Id = WorkspaceId1,
            Name = "Test",
            Address = "Test",
            Description = "Test",
            OwnerId = Guid.NewGuid().ToString(),
            Owner = new AppUser(),
            Rooms = new List<Room>()
        };
        
        var workspace2 = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = "Test2",
            Address = "Test2",
            Description = "Test2",
            OwnerId = Guid.NewGuid().ToString(),
            Owner = new AppUser(),
            Rooms = new List<Room>()
        };
        
        context.Workspaces.AddRange(workspace1, workspace2);
        context.SaveChanges();
    }

    [Test]
    public async Task GetAllWorkspaces_WhenWorkspacesExists_ShouldReturnListWithWorkspace()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var mockProducer = new Mock<IWorkspaceProducer>();
        var repository = new WorkspaceRepository(context, mockProducer.Object);
        
        var res = await repository.GetAllWorkspaces();
        
        Assert.That(res, Is.Not.Null);
        Assert.That(res.Count, Is.EqualTo(2));
        Assert.That(res.Any(w => w.Id == WorkspaceId1), Is.True);
        Assert.That(res.Any(w => w.Name == "Test2"), Is.True);
    }

    [Test]
    public async Task GetWorkspaceById_WhenWorkspaceExists_ShouldReturnWorkspace()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var mockProducer = new Mock<IWorkspaceProducer>();
        var repository = new WorkspaceRepository(context, mockProducer.Object);

        var workspace = await repository.GetWorkspaceById(WorkspaceId1);
        
        Assert.That(workspace, Is.Not.Null);
        Assert.That(workspace, Is.InstanceOf<Workspace>());
    }
    
    [Test]
    public async Task GetWorkspaceById_WhenWorkspaceDoesNotExists_ShouldReturnNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var mockProducer = new Mock<IWorkspaceProducer>();
        var repository = new WorkspaceRepository(context, mockProducer.Object);
        var id = Guid.Empty;
        
        var ex = Assert.ThrowsAsync<CustomExceptions.WorkspaceNotFoundException>(async () =>
        {
            await repository.GetWorkspaceById(id);
        });

        Assert.That(ex.Message, Is.EqualTo($"Workspace с id @{id} не найдено"));
    }

    [Test]
    public async Task DeleteWorkspace_WhenWorkspaceExist_ShouldDeleteWorkspace()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var mockProducer = new Mock<IWorkspaceProducer>();
        var repository = new WorkspaceRepository(context, mockProducer.Object);
        
        var workspace = await repository.DeleteWorkspace(WorkspaceId1);
        var deletedWorkspace = await context.Workspaces.FindAsync(WorkspaceId1);
        
        Assert.That(workspace, Is.Not.Null);
        Assert.That(deletedWorkspace, Is.Null);
        Assert.That(workspace, Is.InstanceOf<Workspace>());
        
        mockProducer.Verify(p => p.WorkspaceDeleteProducer(It.Is<WorkspaceEvent>(
            e => e.WorkspaceId == WorkspaceId1 && e.Type == "workspace.deleted")), Times.Once);
    }
    
    [Test]
    public async Task DeleteWorkspace_WhenWorkspaceDoesNotExist_ShouldReturnNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var mockProducer = new Mock<IWorkspaceProducer>();
        var repository = new WorkspaceRepository(context, mockProducer.Object);
        var id = Guid.Empty;
        
        var ex = Assert.ThrowsAsync<CustomExceptions.WorkspaceNotFoundException>(async () =>
        {
            await repository.DeleteWorkspace(id);
        });

        Assert.That(ex.Message, Is.EqualTo($"Workspace с id @{id} не найдено"));
    }
}