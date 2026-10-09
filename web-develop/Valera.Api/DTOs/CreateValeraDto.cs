namespace Valera.Api.DTOs;

public record CreateValeraDto(
    int Health = 100,
    int Mana = 0,
    int Cheerfulness = 0,
    int Fatigue = 0,
    int Money = 0);