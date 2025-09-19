using Backend.Dto.AccountDto;
using Backend.Dto.BookingDto;
using Backend.Interfaces;
using Backend.MediatR.Queries.Booking;
using MediatR;

namespace Backend.MediatR.Handlers.Booking;

public class GetUserBookingQueryHandler(IBookingRepository bookingRepository) : IRequestHandler<GetUserBookingsQuery, List<ReturnBooking>>
{
    public async Task<List<ReturnBooking>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await bookingRepository.GetUserBookings(request.UserId);

        var bookingsForReturn = new List<ReturnBooking>();

        foreach (var booking in bookings)
        {
            var userForReturn = new BookingReturnUserDto
            {
                UserName = booking.User.UserName,
                Email = booking.User.Email
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
            
            bookingsForReturn.Add(bookingForReturn);
        }

        return bookingsForReturn;
    }
}