using Backend.Dto.BookingDto;
using MediatR;

namespace Backend.MediatR.Commands.Booking;

public class CancelBookingCommand : IRequest<ReturnBooking>
{
    public Guid Id { get; set; }
}