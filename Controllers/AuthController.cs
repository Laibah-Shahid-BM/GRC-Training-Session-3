using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookApi2.Services;

namespace MyBookApi2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public record LoginRequest(string Username, string Password);

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!_authService.ValidateCredentials(request.Username, request.Password, out var role))
            return Unauthorized(new { message = "Invalid username or password." });

        var token = _authService.GenerateToken(request.Username, role);
        return Ok(new { token });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            username = User.Identity?.Name,
            role     = User.FindFirstValue(ClaimTypes.Role),
            claims
        });
    }
}
