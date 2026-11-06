using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.DTOs;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Services;

public class ValeraService
{
    private readonly AppDbContext _db;
    public ValeraService(AppDbContext db) => _db = db;

    // 1. Для Админа: получить всех
    public async Task<List<ValeraStateDto>> GetAllAsync() =>
        await _db.Valeras.Select(v => ValeraStateDto.From(v)).ToListAsync();

    // 2. Для Юзера: получить только СВОИХ
    public async Task<List<ValeraStateDto>> GetByUserIdAsync(int userId) =>
        await _db.Valeras
            .Where(v => v.UserId == userId)
            .Select(v => ValeraStateDto.From(v))
            .ToListAsync();

    // 3. Получить по ID
    public async Task<ValeraStateDto?> GetByIdAsync(int id)
    {
        var v = await _db.Valeras.FindAsync(id);
        return v is null ? null : ValeraStateDto.From(v);
    }

    // 4. Создать (с привязкой к userId)
    public async Task<ValeraStateDto> CreateAsync(CreateValeraDto dto, int userId)
    {
        var v = new ValeraEntity(
            name: dto.Name,
            health: dto.Health,
            mana: dto.Mana,
            cheerfulness: dto.Cheerfulness,
            fatigue: dto.Fatigue,
            money: dto.Money,
            userId: userId); // <-- Передаем владельца

        _db.Valeras.Add(v);
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    // 5. Выполнить действие
    public async Task<ValeraStateDto?> ExecuteActionAsync(int id, Action<ValeraEntity> action)
    {
        var v = await _db.Valeras.FindAsync(id);
        if (v is null) return null;

        action(v);
        await _db.SaveChangesAsync();
        return ValeraStateDto.From(v);
    }

    // 6. Удалить
    public async Task<bool> DeleteAsync(int id)
    {
        var v = await _db.Valeras.FindAsync(id);
        if (v is null) return false;

        _db.Valeras.Remove(v);
        await _db.SaveChangesAsync();
        return true;
    }
}