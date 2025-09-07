using Backend.Dto.AccountDto;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.MediatR.Commands.Account;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.MediatR.Handlers.Account;

public class RegisterNewUserCommandHandler(UserManager<AppUser> userManager, IAccountService accountService, ITokenService tokenService)
    : IRequestHandler<RegisterNewUserCommand, ReturnAppUser>
{
    public async Task<ReturnAppUser> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
    {
        await accountService.CheckNewUser(request);

        var appUser = new AppUser
        {
            UserName = request.Username,
            Email = request.Email
        };
        var createdUser = await userManager.CreateAsync(appUser, request.Password);
        if (createdUser.Succeeded)
        {
            return new ReturnAppUser
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = tokenService.CreateToken(appUser)
            };
        }else
        {
            var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
            throw new CustomExceptions.InternalServerErrorException(errors);
        }
    }
}