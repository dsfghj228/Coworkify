using Backend.MediatR.Commands.Workspace;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Workspace;

public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}