using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.DTOs;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Services;

public class ValeraService
{
    private readonly AppDbContext _db;

    public ValeraService(AppDbContext db) => _db = db;

    private async Task<ValeraEntity> GetOrCreateAsync()
    {
        var v = await _db.Valeras.FirstOrDefaultAsync();
        if (v is null)
        {
            v = new ValeraEntity();
            _db.Valeras.Add(v);
            await _db.SaveChangesAsync();
        }
        return v;
    }

    public async Task<ValeraStateDto> GetStateAsync()
    {
        var v = await GetOrCreateAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> CreateAsync(CreateValeraDto dto)
    {
        var existing = await _db.Valeras.FirstOrDefaultAsync();
        if (existing is not null)
            _db.Valeras.Remove(existing);

        var v = new ValeraEntity(
            health: dto.Health,
            mana: dto.Mana,
            cheerfulness: dto.Cheerfulness,
            fatigue: dto.Fatigue,
            money: dto.Money);

        _db.Valeras.Add(v);
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> GoToWorkAsync()
    {
        var v = await GetOrCreateAsync();
        v.GoToWork();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> ContemplateNatureAsync()
    {
        var v = await GetOrCreateAsync();
        v.ContemplateNature();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> DrinkWineAndWatchSeriesAsync()
    {
        var v = await GetOrCreateAsync();
        v.DrinkWineAndWatchSeries();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> GoToBarAsync()
    {
        var v = await GetOrCreateAsync();
        v.GoToBar();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> DrinkWithMarginalsAsync()
    {
        var v = await GetOrCreateAsync();
        v.DrinkWithMarginals();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> SingInMetroAsync()
    {
        var v = await GetOrCreateAsync();
        v.SingInMetro();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    public async Task<ValeraStateDto> SleepAsync()
    {
        var v = await GetOrCreateAsync();
        v.Sleep();
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }
}