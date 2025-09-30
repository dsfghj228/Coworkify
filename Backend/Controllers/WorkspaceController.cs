using System.Security.Claims;
using Backend.Dto.WorkspaceDto;
using Backend.Exceptions;
using Backend.MediatR.Commands.Workspace;
using Backend.MediatR.Queries.Workspace;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/workspace")]
[ApiController]
public class WorkspaceController(IMediator mediator, UserManager<AppUser> userManager, ILogger<WorkspaceController> logger) : ControllerBase
{
    /// <summary>
    /// Контроллер создания рабочего пространства
    /// </summary>
    /// <param name="workspace">Параметры</param>
    /// <response code="200">Успешное создание</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateWorkspace workspace)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new CustomExceptions.UnauthorizedException();
        }

        var newWorkspace = new CreateWorkspaceCommand
        {
            Owner = user,
            Address = workspace.Address,
            Description = workspace.Description,
            Name = workspace.Name
        };
        
        var result = await mediator.Send(newWorkspace);
        logger.LogInformation("Успешное создание workspace");
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер получения всех достпуных рабочих пространств
    /// </summary>
    /// <response code="200">Успешное получение</response>
    [HttpGet]
    public async Task<IActionResult> GetAllWorkspaces()
    {
        var query = new GetAllWorkspacesQuery();
        var result = await mediator.Send(query);
        logger.LogInformation("Успешное получение всех workspaces");
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для получения рабочего пространства по Id
    /// </summary>
    /// <param name="id">Параметры</param>
    /// <response code="200">Успешное получение рабочего пространства по Id</response>
    /// <response code="404">Рабочее пространство с таким id не найдена</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceById([FromRoute] Guid id)
    {
        var query = new GetWorkspaceByIdQuery
        {
            Id = id
        };
        var result = await mediator.Send(query);
        logger.LogInformation("Workspace c id {Id} успешно найден", id);
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для удаления рабочего простанства
    /// </summary>
    /// <param name="id">Параметры</param>
    /// <response code="200">Успешное удаление пространства</response>
    /// <response code="404">Рабочее пространство с таким id не найдена</response>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteWorkspace([FromRoute] Guid id)
    {
        var command = new DeleteWorkspaceCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);
        logger.LogInformation("Успешное удаление workspace c Id: {Id}", id);
        return Ok(result);
    }
}