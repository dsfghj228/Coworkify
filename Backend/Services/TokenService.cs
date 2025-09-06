using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class TokenService(IConfiguration configuration, ILogger<TokenService> logger) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.GivenName, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        logger.LogInformation("Успешное создание токена");
        return tokenHandler.WriteToken(token);
    }
}