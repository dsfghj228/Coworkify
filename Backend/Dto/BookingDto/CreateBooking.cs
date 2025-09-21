namespace Backend.Dto.BookingDto;

public class CreateBooking
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomId { get; set; }
    public decimal TotalPrice { get; set; }
}