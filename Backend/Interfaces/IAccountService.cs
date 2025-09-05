using Backend.MediatR.Commands.Account;

namespace Backend.Interfaces;

public interface IAccountService
{
    Task CheckNewUser(RegisterNewUserCommand request);
}