using Backend.Dto.AccountDto;
using Backend.Dto.BookingDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Booking;
using MediatR;

namespace Backend.MediatR.Handlers.Booking;

public class CancelBookingCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<CancelBookingCommand, ReturnBooking>
{
    public async Task<ReturnBooking> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.CancelBooking(request.Id);

        var userForReturn = new BookingReturnUserDto
        {
            Email = booking.User.Email,
            UserName = booking.User.UserName
        };

        var bookingForReturn = new ReturnBooking
        {
            Id = booking.Id,
            StartDate = booking.StartTime,
            EndDate = booking.EndTime,
            Status = booking.Status,
            RoomId = booking.RoomId,
            User = userForReturn,
            TotalPrice = booking.TotalPrice
        };

        return bookingForReturn;
    }
}