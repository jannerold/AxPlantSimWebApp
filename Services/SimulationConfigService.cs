using AxPlantSimWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AxPlantSimWebApp.Services;

public class SimulationConfigService
{
  private readonly AppDbContext _db;

  public SimulationConfigService(AppDbContext db)
  {
    _db = db;
  }

  // ------------------------------------
  // Načtení konfigurace (může být prázdná)
  // ------------------------------------
  public async Task<SimulationConfig> GetAsync()
  {
    var entity = await _db.SimulationConfigs
      .AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == 1);

    // pokud v DB ještě není žádná konfigurace,
    // vrátíme prázdný objekt (NEukládáme ho)
    if (entity == null)
    {
      return new SimulationConfig
      {
        Id = 1
        // ostatní pole zůstanou default (null / 0 / DateTime?)
      };
    }

    return entity;
  }

  // ------------------------------------
  // Uložení konfigurace (insert nebo update)
  // ------------------------------------
  public async Task SaveAsync(SimulationConfig incoming)
  {
    var entity = await _db.SimulationConfigs
      .SingleOrDefaultAsync(x => x.Id == 1);

    if (entity == null)
    {
      // první uložení → INSERT
      incoming.Id = 1;
      incoming.UpdatedAt = DateTime.UtcNow;

      _db.SimulationConfigs.Add(incoming);
    }
    else
    {
      // další uložení → UPDATE
      entity.StartTime = incoming.StartTime;
      entity.SimulationTime = incoming.SimulationTime;
      entity.DeadlineDays = incoming.DeadlineDays;
      entity.ReplacementCalendarDays = incoming.ReplacementCalendarDays;
      entity.MaterialLeadTimeHours = incoming.MaterialLeadTimeHours;
      entity.UpdatedAt = DateTime.UtcNow;
    }

    await _db.SaveChangesAsync();
  }
}
