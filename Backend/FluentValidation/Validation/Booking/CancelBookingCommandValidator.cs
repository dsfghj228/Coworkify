using Backend.MediatR.Commands.Booking;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Booking;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}