using System.Security.Claims;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Account;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Backend.Services;

public class AccountService(UserManager<AppUser> userManager) : IAccountService
{
    public async Task CheckNewUser(RegisterNewUserCommand request)
    {
        var existingUser = await userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            throw new CustomExceptions.UserAlreadyExistsException();
        }
    }

    public async Task<AppUser> CheckLoginUser(LoginUserCommand request)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
        if (user == null)
        {
            throw new CustomExceptions.UnauthorizedUsernameException();
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new CustomExceptions.UnauthorizedPasswordException();
        }

        return user;
    }
}