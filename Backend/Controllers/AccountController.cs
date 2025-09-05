using Backend.Dto.AccountDto;
using Backend.MediatR.Commands.Account;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("coworkify/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAppUser registerModel)
    {
        var newUser = new RegisterNewUserCommand
        {
            Username = registerModel.Username,
            Email = registerModel.Email,
            Password = registerModel.Password
        };
        
        var result =  await _mediator.Send(newUser);
        return Ok(result);
    }
}