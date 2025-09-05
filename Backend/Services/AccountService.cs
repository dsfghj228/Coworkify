using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Account;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services;

public class AccountService(UserManager<AppUser> userManager) : IAccountService
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task CheckNewUser(RegisterNewUserCommand request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            throw new CustomExceptions.UserAlreadyExistsException();
        }
    }
}