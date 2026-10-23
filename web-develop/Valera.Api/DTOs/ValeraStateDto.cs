using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.DTOs;

public record ValeraStateDto(
    int Id,
    string Name,
    int Health,
    int Mana,
    int Cheerfulness,
    int Fatigue,
    int Money)
{
    public static ValeraStateDto From(ValeraEntity v) =>
        new(v.Id, v.Name, v.Health, v.Mana, v.Cheerfulness, v.Fatigue, v.Money);
}