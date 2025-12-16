using Microsoft.EntityFrameworkCore;
using AxPlantSimWebApp.Models;

namespace AxPlantSimWebApp.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
    {
    }

    public DbSet<Workplace> Workplaces { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<SimulationConfig> SimulationConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<SimulationConfig>(entity =>
      {
        entity.ToTable("SimulationConfig");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();
      });
    }
  }
}
