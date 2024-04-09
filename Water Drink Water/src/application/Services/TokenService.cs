using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TbdFriends.WaterDrinkWater.Application.Services;

public class JwtService(IConfiguration configuration)
{
    public (string token, DateTime expiration) GenerateToken(string userId, string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["auth:signing-key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
        };

        var expiration = DateTime.UtcNow.AddMinutes(1);


        var token = new JwtSecurityToken(
            issuer: configuration["auth:issuer"],
            audience: configuration["auth:audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}