using System;
using Valera.Api.Models;
using Xunit;

namespace Valera.Tests;

public class ValeraTests
{
    [Fact]
    public void GoToWork_Allowed_ChangesState()
    {
        var v = new Valera.Api.Models.Valera(health: 100, mana: 40, cheerfulness: 0, fatigue: 5, money: 0);

        v.GoToWork();

        Assert.Equal(-5, v.Cheerfulness);
        Assert.Equal(10, v.Mana);
        Assert.Equal(100, v.Money);
        Assert.Equal(75, v.Fatigue);
    }

    [Fact]
    public void GoToWork_NotAllowed_Throws()
    {
        var v = new Valera.Api.Models.Valera(mana: 50, fatigue: 0);

        Assert.Throws<InvalidOperationException>(() => v.GoToWork());
    }

    [Fact]
    public void SingInMetro_WithBonus_Adds60()
    {
        var v = new Valera.Api.Models.Valera(mana: 50, money: 0);

        v.SingInMetro();

        Assert.Equal(60, v.Money);
        Assert.Equal(60, v.Mana);
        Assert.Equal(1, v.Cheerfulness);
        Assert.Equal(20, v.Fatigue);
    }

    [Fact]
    public void SingInMetro_WithoutBonus_Adds10()
    {
        var v = new Valera.Api.Models.Valera(mana: 30, money: 0);

        v.SingInMetro();

        Assert.Equal(10, v.Money);
        Assert.Equal(40, v.Mana);
    }

    [Fact]
    public void Sleep_LowAlcohol_AddsHealth()
    {
        var v = new Valera.Api.Models.Valera(health: 10, mana: 20, cheerfulness: 0, fatigue: 80);

        v.Sleep();

        Assert.Equal(100, v.Health);
        Assert.Equal(0, v.Mana);
        Assert.Equal(0, v.Cheerfulness);
        Assert.Equal(10, v.Fatigue);
    }

    [Fact]
    public void Sleep_HighAlcohol_DecreasesCheerfulness()
    {
        var v = new Valera.Api.Models.Valera(health: 50, mana: 80, cheerfulness: 0, fatigue: 80);

        v.Sleep();

        Assert.Equal(50, v.Health);
        Assert.Equal(30, v.Mana);
        Assert.Equal(-3, v.Cheerfulness);
        Assert.Equal(10, v.Fatigue);
    }

    [Fact]
    public void Values_AreClamped()
    {
        var v = new Valera.Api.Models.Valera(
            health: 150,
            mana: -10,
            cheerfulness: 20,
            fatigue: 200,
            money: 0);

        Assert.Equal(100, v.Health);
        Assert.Equal(0, v.Mana);
        Assert.Equal(10, v.Cheerfulness);
        Assert.Equal(100, v.Fatigue);
    }
}