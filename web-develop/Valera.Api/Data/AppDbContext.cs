using Microsoft.EntityFrameworkCore;
using ValeraEntity = Valera.Api.Models.Valera;

namespace Valera.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<ValeraEntity> Valeras => Set<ValeraEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
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
    }
}