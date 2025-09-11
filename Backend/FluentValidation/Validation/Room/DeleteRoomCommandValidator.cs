using Backend.MediatR.Commands.Room;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Room;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}