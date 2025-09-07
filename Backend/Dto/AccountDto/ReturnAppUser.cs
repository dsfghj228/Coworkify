namespace Backend.Dto.AccountDto;

public class ReturnAppUser
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string Token { get; set; }
}