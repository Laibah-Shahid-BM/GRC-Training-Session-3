using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyBookApi2.Services;

public class AuthService : IAuthService
{
    private static readonly Dictionary<string, (string Password, string Role)> _users = new()
    {
        { "admin", ("admin123", "Admin") },
        { "user",  ("user123",  "User")  }
    };

    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public bool ValidateCredentials(string username, string password, out string role)
    {
        role = string.Empty;
        if (!_users.TryGetValue(username, out var userData) || userData.Password != password)
            return false;

        role = userData.Role;
        return true;
    }

    public string GenerateToken(string username, string role)
    {
        var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
