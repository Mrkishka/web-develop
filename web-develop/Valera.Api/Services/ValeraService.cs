using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.DTOs;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Services;

public class ValeraService
{
    private readonly AppDbContext _db;

    public ValeraService(AppDbContext db) => _db = db;

    // 1. Получить список всех Валер
    public async Task<List<ValeraStateDto>> GetAllAsync()
    {
        var valeras = await _db.Valeras.ToListAsync();
        return valeras.Select(ValeraStateDto.From).ToList();
    }

    // 2. Получить одного Валеру по ID
    public async Task<ValeraStateDto?> GetByIdAsync(int id)
    {
        var v = await _db.Valeras.FindAsync(id);
        return v is null ? null : ValeraStateDto.From(v);
    }

    // 3. Создать нового Валеру
    public async Task<ValeraStateDto> CreateAsync(CreateValeraDto dto)
    {
        var v = new ValeraEntity(
            name: dto.Name,
            health: dto.Health,
            mana: dto.Mana,
            cheerfulness: dto.Cheerfulness,
            fatigue: dto.Fatigue,
            money: dto.Money);

        _db.Valeras.Add(v);
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    // 4. Универсальный метод для выполнения любого действия (work, sleep и т.д.)
    public async Task<ValeraStateDto?> ExecuteActionAsync(int id, Action<ValeraEntity> action)
    {
        var v = await _db.Valeras.FindAsync(id);
        if (v is null) return null;

        // Вызываем переданный метод модели (например, v.GoToWork())
        action(v); 
        
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }
}