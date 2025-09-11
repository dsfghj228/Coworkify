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
}