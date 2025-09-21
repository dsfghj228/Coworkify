using Backend.MediatR.Commands.Booking;
using Backend.Models;

namespace Backend.Interfaces;

public interface IBookingRepository
{
    Task<Booking> CreateBooking(BookRoomCommand command);
    Task<Booking> CancelBooking(Guid id);
    Task<List<Booking>> GetUserBookings(string UserId);
    Task CompleteBookings();
}