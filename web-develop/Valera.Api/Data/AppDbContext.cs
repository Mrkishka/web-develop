using Microsoft.EntityFrameworkCore;
using Valera.Api.Models;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<ValeraEntity> Valeras => Set<ValeraEntity>();
    
    // Добавлен DbSet для пользователей
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Настройка Valera
        mb.Entity<ValeraEntity>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Health).IsRequired();
            e.Property(v => v.Mana).IsRequired();
            e.Property(v => v.Cheerfulness).IsRequired();
            e.Property(v => v.Fatigue).IsRequired();
            e.Property(v => v.Money).IsRequired();
            e.ToTable("Valeras");
        });

        // Настройка User
        mb.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            // Email должен быть уникальным (нельзя зарегистрировать 2 раза)
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired();
            e.Property(u => u.Username).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Role).IsRequired();
            e.ToTable("Users");
        });
    }
}