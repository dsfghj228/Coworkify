using System.Security.Claims;
using Backend.Dto.BookingDto;
using Backend.Exceptions;
using Backend.MediatR.Commands.Booking;
using Backend.MediatR.Queries.Booking;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/booking")]
[ApiController]
public class BookingController(UserManager<AppUser> userManager, IMediator mediator, ILogger<BookingController> logger) : ControllerBase
{
    /// <summary>
    /// Контроллер для бронирования комнаты
    /// </summary>
    /// <param name="newBooking">Параметры</param>
    /// <response code="200">Успешное бронирование</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="404">Комната не найдена</response>
    /// <response code="409">Нельзя забронировать комнату на эту дату</response>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> BookRoom([FromBody] CreateBooking newBooking)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new CustomExceptions.UnauthorizedException();
        }
        
        var command = new BookRoomCommand
        {
            StartDate = newBooking.StartDate,
            EndDate = newBooking.EndDate,
            RoomId = newBooking.RoomId,
            UserId = userId,
            TotalPrice = newBooking.TotalPrice,
            User = user
        };
        var result = await mediator.Send(command);
        logger.LogInformation("Успешное бронирование комнаты с Id {@Id} на период с {@StartDate} по {@EndDate}", userId, newBooking.StartDate, newBooking.EndDate);
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для отмены бронирования комнаты
    /// </summary>
    /// <param name="id">Параметры</param>
    /// <response code="200">Успешная отмена бронирования</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="404">Бронирование с таким id не найдена</response>
    /// <response code="409">Нельзя отменить бронирование</response>
    [HttpPut("cancel/{id}")]
    [Authorize]
    public async Task<IActionResult> CancelBooking([FromRoute] Guid id)
    {
        var command = new CancelBookingCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);
        logger.LogInformation("Успешная отмена брони с id: {@Id}", id);
        return Ok(result);
    }
    
    /// <summary>
    /// Контроллер для получения бронирований пользователя
    /// </summary>
    /// <response code="200">Успешное получение всех бронирований</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            throw new CustomExceptions.UnauthorizedException();
        }

        var query = new GetUserBookingsQuery
        {
            UserId = userId
        };
        
        var result = await mediator.Send(query);
        logger.LogInformation("Успешное получение всех бронирований пользователя с Id: {@Id}", userId);
        return Ok(result);
    }
}