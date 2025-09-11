using Backend.MediatR.Queries.Room;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Room;

public class GetAllRoomsQueryValidator  : AbstractValidator<GetAllRoomsQuery>
{
    public GetAllRoomsQueryValidator()
    {
        RuleFor(r => r.WorkspaceId).NotEmpty();
    }
}