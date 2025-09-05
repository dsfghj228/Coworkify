using System.Net;

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

    public class InternalServerErrorException() : CustomExceptions(HttpStatusCode.InternalServerError,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Внутренняя ошибка сервера",
        "Произошла непредвиденная ошибка на сервере. Пожалуйста, попробуйте позже или обратитесь в поддержку.");
}