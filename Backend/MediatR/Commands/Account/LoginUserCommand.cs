using Backend.Dto.AccountDto;
using MediatR;

namespace Backend.MediatR.Commands.Account;

public class LoginUserCommand : IRequest<ReturnAppUser>
{
    public string Username { get; set; }
    public string Password { get; set; }
}