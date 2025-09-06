using Backend.Dto.AccountDto;
using Backend.Interfaces;
using Backend.MediatR.Commands.Account;
using MediatR;

namespace Backend.MediatR.Handlers.Account;

public class LoginUserCommandHandler(IAccountService accountService, ITokenService tokenService) : IRequestHandler<LoginUserCommand, ReturnAppUser>
{
    public async Task<ReturnAppUser> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await accountService.CheckLoginUser(request);
        
        return new ReturnAppUser
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
}