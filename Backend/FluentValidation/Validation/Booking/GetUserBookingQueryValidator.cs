using Backend.MediatR.Queries.Booking;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Booking;

public class GetUserBookingQueryValidator : AbstractValidator<GetUserBookingsQuery>
{
    public GetUserBookingQueryValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}