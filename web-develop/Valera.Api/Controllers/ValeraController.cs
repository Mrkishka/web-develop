using Microsoft.AspNetCore.Mvc;
using Valera.Api.DTOs;
using Valera.Api.Services;

namespace Valera.Api.Controllers;

[ApiController]
[Route("api/valera")]
[Produces("application/json")]
public class ValeraController : ControllerBase
{
    private readonly ValeraService _service;

    public ValeraController(ValeraService service) => _service = service;

    /// <summary>Текущее состояние Валеры.</summary>
    [HttpGet]
    public async Task<ActionResult<ValeraStateDto>> Get() =>
        Ok(await _service.GetStateAsync());

    /// <summary>Создать/пересоздать Валеру с указанными параметрами.</summary>
    [HttpPost]
    public async Task<ActionResult<ValeraStateDto>> Create([FromBody] CreateValeraDto dto) =>
        Ok(await _service.CreateAsync(dto));

    /// <summary>Пойти на работу (Mana &lt; 50, Fatigue &lt; 10).</summary>
    [HttpPost("work")]
    public async Task<ActionResult<ValeraStateDto>> Work() =>
        Ok(await _service.GoToWorkAsync());

    /// <summary>Созерцать природу.</summary>
    [HttpPost("contemplate")]
    public async Task<ActionResult<ValeraStateDto>> Contemplate() =>
        Ok(await _service.ContemplateNatureAsync());

    /// <summary>Пить вино и смотреть сериал.</summary>
    [HttpPost("drink-wine")]
    public async Task<ActionResult<ValeraStateDto>> DrinkWine() =>
        Ok(await _service.DrinkWineAndWatchSeriesAsync());

    /// <summary>Сходить в бар.</summary>
    [HttpPost("bar")]
    public async Task<ActionResult<ValeraStateDto>> Bar() =>
        Ok(await _service.GoToBarAsync());

    /// <summary>Выпить с маргинальными личностями.</summary>
    [HttpPost("drink-marginals")]
    public async Task<ActionResult<ValeraStateDto>> DrinkMarginals() =>
        Ok(await _service.DrinkWithMarginalsAsync());

    /// <summary>Петь в метро.</summary>
    [HttpPost("sing")]
    public async Task<ActionResult<ValeraStateDto>> Sing() =>
        Ok(await _service.SingInMetroAsync());

    /// <summary>Спать.</summary>
    [HttpPost("sleep")]
    public async Task<ActionResult<ValeraStateDto>> Sleep() =>
        Ok(await _service.SleepAsync());
}