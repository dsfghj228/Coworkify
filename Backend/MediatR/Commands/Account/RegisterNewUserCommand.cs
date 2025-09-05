using Backend.Dto.AccountDto;
using Backend.Models;
using MediatR;

namespace Backend.MediatR.Commands.Account;

public class RegisterNewUserCommand : IRequest<AppUser>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}