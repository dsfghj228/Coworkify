using Backend.Dto.BookingDto;
using Backend.Models;
using MediatR;

namespace Backend.MediatR.Commands.Booking;

public class BookRoomCommand : IRequest<ReturnBooking>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public Guid RoomId { get; set; }
    public decimal TotalPrice { get; set; }
}