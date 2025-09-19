using Backend.Dto.AccountDto;
using Backend.Dto.BookingDto;
using Backend.Dto.RoomDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Booking;
using MediatR;

namespace Backend.MediatR.Handlers.Booking;

public class BookRoomCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<BookRoomCommand, ReturnBooking>
{
    public async Task<ReturnBooking> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.CreateBooking(request);

        var userForReturn = new BookingReturnUserDto
        {
            UserName = booking.User.UserName,
            Email = booking.User.Email,
        };

        var bookingForReturn = new ReturnBooking
        {
            Id = booking.Id,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = booking.Status,
            User = userForReturn,
            RoomId = request.RoomId,
            TotalPrice = request.TotalPrice
        };
        return bookingForReturn;
    }
}