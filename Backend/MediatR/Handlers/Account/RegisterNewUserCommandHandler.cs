using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Account;
using Backend.Models;
using Backend.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.MediatR.Handlers.Account;

public class RegisterNewUserCommandHandler(UserManager<AppUser> userManager, AccountService accountService)
    : IRequestHandler<RegisterNewUserCommand, AppUser>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IAccountService _accountService = accountService;

    public async Task<AppUser> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
    {
        await _accountService.CheckNewUser(request);

        var appUser = new AppUser
        {
            UserName = request.Username,
            Email = request.Email
        };
        var createdUser = await _userManager.CreateAsync(appUser, request.Password);
        if (createdUser.Succeeded)
        {
            return appUser;
        }
        else
        {
            throw new CustomExceptions.InternalServerErrorException();
        }
    }
}