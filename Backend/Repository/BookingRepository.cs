using Backend.Data;
using Backend.Enums;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Booking;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task<Booking> CreateBooking(BookRoomCommand command)
    {
        var alreadyExistsBooking = await context.Bookings
            .AnyAsync(b =>
                b.RoomId == command.RoomId &&
                b.StartTime < command.EndDate &&
                b.EndTime > command.StartDate);
        
        if (alreadyExistsBooking)
        {
            throw new CustomExceptions.BookingArleadyExistsException(command.RoomId);
        }
        
        var room = await context.Rooms
            .Include(room => room.Bookings)
            .Where(r => r.Id == command.RoomId)
            .FirstOrDefaultAsync();
        if (room == null)
        {
            throw new CustomExceptions.RoomNotFoundException(command.RoomId);
        }

        var booking = new Booking
        {
            StartTime = command.StartDate,
            EndTime = command.EndDate,
            Status = BookingStatus.Booked,
            UserId = command.UserId,
            User = command.User,
            RoomId = command.RoomId,
            Room = room,
            TotalPrice = command.TotalPrice,
        };
        
        room.Bookings.Add(booking);
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking> CancelBooking(Guid id)
    {
        var booking = await context.Bookings
            .Where(b => b.Id == id)
            .Include(b => b.User)
            .FirstOrDefaultAsync();

        if (booking == null)
        {
            throw new CustomExceptions.BookingNotFoundException(id);
        }

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
        {
            throw new CustomExceptions.BookingCanNotBeCancelledException(id, booking.Status);
        }

        booking.Status = BookingStatus.Cancelled;
        context.Bookings.Update(booking);
        await context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<Booking>> GetUserBookings(string UserId)
    {
        var bookings = await context.Bookings
            .Where(b => b.UserId == UserId)
            .Include(b => b.User)
            .ToListAsync();

        return bookings;
    }
}