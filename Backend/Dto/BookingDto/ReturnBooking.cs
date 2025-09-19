using Backend.Dto.AccountDto;

namespace Backend.Dto.BookingDto;

public class ReturnBooking
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingReturnUserDto  User { get; set; }
    public Guid RoomId { get; set; }
    public decimal TotalPrice { get; set; }
}