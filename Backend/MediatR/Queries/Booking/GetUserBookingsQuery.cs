using Backend.Dto.BookingDto;
using MediatR;

namespace Backend.MediatR.Queries.Booking;

public class GetUserBookingsQuery : IRequest<List<ReturnBooking>>
{
    public string UserId { get; set; }
}