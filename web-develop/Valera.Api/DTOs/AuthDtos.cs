namespace Valera.Api.DTOs;

public record RegisterDto(string Email, string Username, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string Username, string Role);