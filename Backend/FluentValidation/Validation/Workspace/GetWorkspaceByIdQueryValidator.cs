using Backend.MediatR.Queries.Workspace;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Workspace;

public class GetWorkspaceByIdQueryValidator : AbstractValidator<GetWorkspaceByIdQuery>
{
    public GetWorkspaceByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}