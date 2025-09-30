using Backend.Dto.AccountDto;
using Backend.MediatR.Commands.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("coworkify/account")]
[ApiController]
public class AccountController(IMediator mediator, ILogger<AccountController> logger) : ControllerBase
{
    /// <summary>
    /// Контроллер регистрации новый пользователей
    /// </summary>
    /// <param name="registerModel">Параметры</param>
    /// <response code="200">Новый юзер успешно зарегистрирован</response>
    /// <response code="409">Такой пользователь уже существует</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAppUser registerModel)
    {
        var newUser = new RegisterNewUserCommand
        {
            Username = registerModel.Username,
            Email = registerModel.Email,
            Password = registerModel.Password
        };
        
        var result =  await mediator.Send(newUser);
        logger.LogInformation(
            "Успешное создание пользователя: {@UserResult}", 
            new {result.UserName, result.Email }
        );
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер входа пользователя в систему
    /// </summary>
    /// <param name="loginModel">Параметры</param>
    /// <response code="200">Вход выполнен успешно</response>
    /// <response code="401">
    /// Возможные ошибки:
    /// - Пользователя с таким username не существует
    /// - Неверный пароль
    /// </response>
    /// <response code="409">Такой пользователь уже существует</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAppUser loginModel)
    {
        var loginUser = new LoginUserCommand
        {
            Username = loginModel.Username,
            Password = loginModel.Password
        };
        
        var result =  await mediator.Send(loginUser);
        logger.LogInformation("Успешный вход в учетную запись: {@UserResult}",
            new {result.UserName, result.Email });
        return Ok(result);
    }
}