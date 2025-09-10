using Backend.Dto.RoomDto;
using Backend.MediatR.Commands.Room;
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
}