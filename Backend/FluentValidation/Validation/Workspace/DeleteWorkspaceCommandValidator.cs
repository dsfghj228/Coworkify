using Backend.MediatR.Commands.Workspace;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Workspace;

public class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
{
    public DeleteWorkspaceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}