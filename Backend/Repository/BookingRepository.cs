using Backend.Data;
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
}