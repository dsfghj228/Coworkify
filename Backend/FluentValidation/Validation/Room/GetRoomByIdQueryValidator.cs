using Backend.MediatR.Queries.Room;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Room;

public class GetRoomByIdQueryValidator : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}