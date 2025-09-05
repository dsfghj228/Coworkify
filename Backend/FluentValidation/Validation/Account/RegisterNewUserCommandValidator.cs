using Backend.MediatR.Commands.Account;
using FluentValidation;

namespace Backend.FluentValidation.Validation.Account;

public class RegisterNewUserCommandValidator : AbstractValidator<RegisterNewUserCommand>
{
    public RegisterNewUserCommandValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(request => request.Username).NotEmpty();
    }
}