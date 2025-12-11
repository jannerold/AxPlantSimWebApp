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
  }
}
