using Backend.Dto.AccountDto;
using MediatR;

namespace Backend.MediatR.Commands.Account;

public class RegisterNewUserCommand : IRequest<ReturnAppUser>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}