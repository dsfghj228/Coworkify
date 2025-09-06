using Backend.Models;

namespace Backend.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}