using Backend.Data;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.UnitTests;

[TestFixture]
public class RoomRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> _dbOptions;
    private static readonly Guid WorkspaceId1 = Guid.Parse("fb0cd5d2-1385-4408-9677-a8e0058b0240");
    private static readonly Guid RoomId1 = Guid.Parse("10f05989-8ad7-4b77-a9a8-c07800fe53a7");
    
    [SetUp]
    public void Setup()
    {
        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
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

        var room = new Room
        {
            Id = RoomId1,
            Name = "Test",
            Capacity = 10,
            HourlyRate = 10,
            WorkspaceId = WorkspaceId1,
            Workspace = workspace1,
            Bookings = new List<Booking>()
        };
        context.Rooms.Add(room);
        context.SaveChanges();
    }

    [Test]
    public async Task GetRoomById_WhenRoomExists_ReturnsRoom()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new RoomRepository(context);

        var room = await repository.GetRoomById(RoomId1);
        
        Assert.That(room, Is.Not.Null);
        Assert.That(room.Id, Is.EqualTo(RoomId1));
        Assert.That(room, Is.InstanceOf<Room>());
    }

    [Test]
    public async Task GetRoomById_WhenRoomDoesNotExist_ShouldThrowNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new RoomRepository(context);
        var id = Guid.Empty;
        
        var ex = Assert.ThrowsAsync<CustomExceptions.RoomNotFoundException>(async () =>
        {
            await repository.GetRoomById(id);
        });

        Assert.That(ex.Message, Is.EqualTo($"Room с id @{id} не найдено"));
    }

    [Test]
    public async Task GetAllRooms_ReturnsAllRooms()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new RoomRepository(context);

        var rooms = await repository.GetAllRooms(WorkspaceId1);
        
        Assert.That(rooms, Is.Not.Null);
        Assert.That(rooms, Is.Not.Empty);
        Assert.That(rooms.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteRoom_WhenRoomExists_ShouldDeleteRoom()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new RoomRepository(context);
        
        var room = await repository.DeleteRoom(RoomId1);
        var deletedRoom = await context.Rooms.SingleOrDefaultAsync(r => r.Id == RoomId1);
        
        Assert.That(deletedRoom, Is.Null);
        Assert.That(room, Is.Not.Null);
        Assert.That(room.Id, Is.EqualTo(RoomId1));
        Assert.That(room, Is.InstanceOf<Room>());
    }

    [Test]
    public async Task DeleteRoom_WhenRoomDoesNotExists_ShouldThrowNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new RoomRepository(context);
        var id = Guid.Empty;

        var ex = Assert.ThrowsAsync<CustomExceptions.RoomNotFoundException>(async () =>
        {
            await repository.DeleteRoom(id);
        });
        
        Assert.That(ex.Message, Is.EqualTo($"Room с id @{id} не найдено"));
    }
}