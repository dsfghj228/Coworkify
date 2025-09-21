using System.Net;
using Backend.Enums;

namespace Backend.Exceptions;

public abstract class CustomExceptions : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Type { get; }
    public string Title { get; }

    protected CustomExceptions(
        HttpStatusCode statusCode,
        string type,
        string title,
        string message) : base(message)
    {
        StatusCode = statusCode;
        Type = type;
        Title = title;
    }
    
    public class InvalidNewUserModelException() : CustomExceptions(HttpStatusCode.BadRequest,
        "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "Неправильная модель юзера",
        "Модель нового юзера имеет некорректные поля");
    
    public class UserAlreadyExistsException() : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        "Пользователь уже существует",
        "Такой пользователь уже существует");

    public class InternalServerErrorException(string errors) : CustomExceptions(HttpStatusCode.InternalServerError,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Внутренняя ошибка сервера",
        $"Произошла непредвиденная ошибка на сервере: @{errors}");
    
    public class UnauthorizedUsernameException() : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Ошибка авторизации",
        "При попытке авторизации произошла ошибка. Пользователя с таким username не существует");
    
    public class UnauthorizedPasswordException() : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Ошибка авторизации",
        "При попытке авторизации произошла ошибка. Неверный пароль");
    
    public class UnauthorizedException() : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Ошибка авторизации",
        "При попытке авторизации произошла ошибка.");
    
    public class WorkspaceNotFoundException(Guid id) : CustomExceptions(HttpStatusCode.NotFound,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Не найдено",
        $"Workspace с id @{id} не найдено");
    
    public class RoomNotFoundException(Guid id) : CustomExceptions(HttpStatusCode.NotFound,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Не найдено",
        $"Room с id @{id} не найдено");
    
    public class BookingNotFoundException(Guid id) : CustomExceptions(HttpStatusCode.NotFound,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Не найдено",
        $"Booking с id @{id} не найден");
    
    public class BookingCanNotBeCancelledException(Guid id, BookingStatus status) : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Booking не может быть отменен",
        $"Booking с id @{id} не может быть отменен так как его статус: @{status}");
    
    public class BookingArleadyExistsException(Guid roomId) : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Нельзя забронировать комнату на эту дату",
        $"Нельзя забронировать комнату с Id @{roomId} потому что в этот период времени она уже забронирована");
}