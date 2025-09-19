using Backend.MediatR.Commands.Booking;
using Backend.Models;

namespace Backend.Interfaces;

public interface IBookingRepository
{
    Task<Booking> CreateBooking(BookRoomCommand command);
}