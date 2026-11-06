using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valera.Api.DTOs;
using Valera.Api.Services;

namespace Valera.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Разрешаем доступ без токена
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var success = await _authService.RegisterAsync(dto);
        return success ? Ok(new { message = "Успешно" }) : BadRequest(new { error = "Email уже занят" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);
        return response is null ? Unauthorized(new { error = "Неверный логин или пароль" }) : Ok(response);
    }
}