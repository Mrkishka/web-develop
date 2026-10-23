using Microsoft.AspNetCore.Mvc;
using Valera.Api.DTOs;
using Valera.Api.Services;

namespace Valera.Api.Controllers;

[ApiController]
[Route("api/valera")]
public class ValeraController : ControllerBase
{
    private readonly ValeraService _service;
    public ValeraController(ValeraService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var v = await _service.GetByIdAsync(id);
        return v is null ? NotFound() : Ok(v);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateValeraDto dto) =>
        Ok(await _service.CreateAsync(dto));

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

    private async Task<IActionResult> Action(int id, Action<Valera.Api.Models.Valera> action)
    {
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