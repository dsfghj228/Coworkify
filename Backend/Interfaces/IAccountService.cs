using Backend.MediatR.Commands.Account;
using Backend.Models;

namespace Backend.Interfaces;

public interface IAccountService
{
    Task CheckNewUser(RegisterNewUserCommand request);
    Task<AppUser> CheckLoginUser(LoginUserCommand request);
}