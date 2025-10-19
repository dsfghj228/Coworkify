using Backend.Data;
using Backend.Enums;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.UnitTests;

[TestFixture]
public class BookingRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> _dbOptions;
    private static readonly Guid WorkspaceId1 = Guid.Parse("fb0cd5d2-1385-4408-9677-a8e0058b0240");
    private static readonly Guid RoomId1 = Guid.Parse("10f05989-8ad7-4b77-a9a8-c07800fe53a7");
    private static readonly string UserId = "5ed12af3-8c01-4b4e-bae3-fb24f131df64";
    private static readonly Guid BookingId = Guid.Parse("0c8b931e-5500-4dc0-b422-6da915cd2b69");
    
    [SetUp]
    public void Setup()
    {
        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ApplicationDbContext(_dbOptions);
        
        var user = new AppUser { Id = UserId, UserName = "testuser" };
        context.Users.Add(user);
        
        var workspace1 = new Workspace
        {
            Id = WorkspaceId1,
            Name = "Test",
            Address = "Test",
            Description = "Test",
            OwnerId = Guid.NewGuid().ToString(),
            Owner = user,
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

        var booking = new Booking
        {
            Id = BookingId,
            UserId = user.Id,
            RoomId = RoomId1,
            Room = room,
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow.AddMinutes(-10),
            Status = BookingStatus.Booked,
            TotalPrice = 100,
            User = user,
        };
        
        context.Bookings.Add(booking);
        context.SaveChanges();
    }

    [Test]
    public async Task GetUserBookings_WhenBookingsExists_ReturnsBookings()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new BookingRepository(context);
        
        var bookings = await repository.GetUserBookings(UserId);
        
        Assert.That(bookings, Is.Not.Null);
        Assert.That(bookings, Is.Not.Empty);
        Assert.That(bookings.Count, Is.EqualTo(1));
        Assert.That(bookings.Any(b => b.Id == BookingId), Is.True);
    }

    [Test]
    public async Task CancelBooking_WhenBookingExists_ReturnsCanceledBooking()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new BookingRepository(context);
        
        var booking = await repository.CancelBooking(BookingId);
        
        Assert.That(booking, Is.Not.Null);
        Assert.That(booking, Is.InstanceOf<Booking>());
        Assert.That(booking.Id, Is.EqualTo(BookingId));
        Assert.That(booking.Status, Is.EqualTo(BookingStatus.Cancelled));
    }
    
    [Test]
    public async Task CancelBooking_WhenBookingDoesNotExists_ThrowsNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new BookingRepository(context);
        var id = Guid.NewGuid();
        
        var ex = Assert.ThrowsAsync<CustomExceptions.BookingNotFoundException>(async () => await repository.CancelBooking(id));
        
        Assert.That(ex.Message, Is.EqualTo($"Booking с id @{id} не найден"));
    }

    [Test]
    public async Task CancelBooking_WhenBookingExistsButAlreadyCancelledOrCompleted_ThrowsNotFoundException()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new BookingRepository(context);
        
        var booking = await repository.CancelBooking(BookingId);
        var ex = Assert.ThrowsAsync<CustomExceptions.BookingCanNotBeCancelledException>(async () => await repository.CancelBooking(BookingId));
        
        Assert.That(ex.Message, Is.EqualTo($"Booking с id @{BookingId} не может быть отменен так как его статус: @{booking.Status}"));
    }
    
    [Test]
    public async Task CompleteBookings_WhenBookingExists_ReturnsCompletedBookings()
    {
        await using var context = new ApplicationDbContext(_dbOptions);
        var repository = new BookingRepository(context);

        await repository.CompleteBookings();
        
        var booking = await context.Bookings.SingleAsync(b => b.Id == BookingId);
        Assert.That(booking.Status, Is.EqualTo(BookingStatus.Completed));
    }
}