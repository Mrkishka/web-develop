using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ДОБАВЛЕНО
using System.Security.Claims;             // ДОБАВЛЕНО
using Valera.Api.DTOs;
using Valera.Api.Services;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Controllers;

[Authorize] // <-- Весь контроллер теперь требует токен!
[ApiController]
[Route("api/valera")]
public class ValeraController : ControllerBase
{
    private readonly ValeraService _service;
    public ValeraController(ValeraService service) => _service = service;

    // Вспомогательные методы для получения данных из JWT-токена
    private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin() => User.IsInRole("Admin");

    // 1. Получить ВСЕХ Валер (Только для Админа)
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    // 2. Получить ТОЛЬКО СВОИХ Валер (Для обычного юзера)
    [HttpGet("my")]
    public async Task<IActionResult> GetMy() => Ok(await _service.GetByUserIdAsync(GetCurrentUserId()));

    // 3. Получить по ID (с проверкой владельца)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var v = await _service.GetByIdAsync(id);
        if (v is null) return NotFound();
        if (!IsAdmin() && v.UserId != GetCurrentUserId()) return Forbid(); // Запрет, если чужой
        return Ok(v);
    }

    // 4. Создать (Привязываем к текущему юзеру)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateValeraDto dto) =>
        Ok(await _service.CreateAsync(dto, GetCurrentUserId()));

    // 5. Удалить (Владелец или Админ)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var v = await _service.GetByIdAsync(id);
        if (v is null) return NotFound();
        if (!IsAdmin() && v.UserId != GetCurrentUserId()) return Forbid();

        await _service.DeleteAsync(id);
        return NoContent();
    }

    // Действия (Оставил для краткости только Work, остальные аналогично)
    [HttpPost("{id}/work")]
    public async Task<IActionResult> Work(int id) => await Action(id, v => v.GoToWork());

    [HttpPost("{id}/contemplate")]
    public async Task<IActionResult> Contemplate(int id) => await Action(id, v => v.ContemplateNature());

    [HttpPost("{id}/drink-wine")]
    public async Task<IActionResult> DrinkWine(int id) => await Action(id, v => v.DrinkWineAndWatchSeries());

    [HttpPost("{id}/bar")]
    public async Task<IActionResult> Bar(int id) => await Action(id, v => v.GoToBar());

    [HttpPost("{id}/drink-marginals")]
    public async Task<IActionResult> DrinkMarginals(int id) => await Action(id, v => v.DrinkWithMarginals());

    [HttpPost("{id}/sing")]
    public async Task<IActionResult> Sing(int id) => await Action(id, v => v.SingInMetro());

    [HttpPost("{id}/sleep")]
    public async Task<IActionResult> Sleep(int id) => await Action(id, v => v.Sleep());

    // Общий обработчик действий с проверкой прав
    private async Task<IActionResult> Action(int id, Action<ValeraEntity> action)
    {
        var v = await _service.GetByIdAsync(id);
        if (v is null) return NotFound();
        
        // ПРОВЕРКА ПРАВ: Если не Админ и Валера не его
        if (!IsAdmin() && v.UserId != GetCurrentUserId()) 
            return Forbid();

        try
        {
            var result = await _service.ExecuteActionAsync(id, action);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}