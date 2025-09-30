using Backend.Dto.RoomDto;
using Backend.MediatR.Commands.Room;
using Backend.MediatR.Queries.Room;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/room")]
[ApiController]
public class RoomController(IMediator mediator, ILogger<RoomController> logger) : ControllerBase
{
    /// <summary>
    /// Контроллер для создания комнаты
    /// </summary>
    /// <param name="createRoom">Параметры</param>
    /// <response code="200">Успешное создание</response>
    /// <response code="404">Рабочее пространство с таким id не найдена</response>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoom createRoom)
    {
        var command = new CreateRoomCommand
        {
            Name = createRoom.Name,
            Capacity = createRoom.Capacity,
            HourlyRate = createRoom.HourlyRate,
            WorkspaceId = createRoom.WorkspaceId
        };
        var result = await mediator.Send(command);
        logger.LogInformation("Успешное создание комнаты");
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для получения комнаты по Id
    /// </summary>
    /// <param name="id">Параметры</param>
    /// <response code="200">Успешное получение комнаты</response>
    /// <response code="404">Комната с таким id не найдена</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomById([FromRoute] Guid id)
    {
        var query = new GetRoomByIdQuery
        {
            Id = id
        };
        
        var result = await mediator.Send(query);
        logger.LogInformation("Успешное получение комнаты с id: {@Id}", id);
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для получения списка комнат по Id рабочего пространства
    /// </summary>
    /// <param name="workspaceId">Параметры</param>
    /// <response code="200">Успешное получение</response>
    [HttpGet("workspaces/{workspaceId}")]
    public async Task<IActionResult> GetRoomsByWorkspaceId([FromRoute] Guid workspaceId)
    {
        var query = new GetAllRoomsQuery()
        {
            WorkspaceId = workspaceId
        };
        var result = await mediator.Send(query);
        logger.LogInformation("Успешное получение всех комнат для workspace c Id: {@Id} ", workspaceId);
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для удаления комнаты
    /// </summary>
    /// <param name="id">Параметры</param>
    /// <response code="200">Успешное удаление</response>
    /// <response code="404">Комната с таким id не найдена</response>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRoom([FromRoute] Guid id)
    {
        var command = new DeleteRoomCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);
        logger.LogInformation("Успешное удаление комнаты с Id: {@Id}", id);
        return Ok(result);
    }
}