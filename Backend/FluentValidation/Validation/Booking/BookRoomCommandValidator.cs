using Backend.MediatR.Commands.Booking;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Booking;

public class BookRoomCommandValidator : AbstractValidator<BookRoomCommand>
{
    public BookRoomCommandValidator()
    {
        RuleFor(c => c.StartDate).NotNull();
        RuleFor(c => c.EndDate).NotNull();
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.RoomId).NotEmpty();
        RuleFor(c => c.TotalPrice).NotEmpty();
    }
}