using Backend.MediatR.Commands.Room;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Room;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .NotEmpty();
        RuleFor(x => x.HourlyRate)
            .GreaterThan(0)
            .NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}