using AxPlantSimWebApp.Services;
using AxPlantSimWebApp.Simulation;
using Microsoft.AspNetCore.Mvc;

namespace AxPlantSimWebApp.Controllers;

[Route("simulation")]
public class SimulationController : Controller
{
  private readonly SimulationService _simulationService;
  private readonly SimulationConfigService _configService;

  public SimulationController(
    SimulationService simulationService,
    SimulationConfigService configService)
  {
    _simulationService = simulationService;
    _configService = configService;
  }

  // ---------------------------------
  // Zobrazení stránky simulace
  // ---------------------------------
  [HttpGet("")]
  public async Task<IActionResult> Index()
  {
    var config = await _configService.GetAsync();
    return View(config);
  }

  // ---------------------------------
  // Realtime autosave konfigurace
  // ---------------------------------
  [HttpPost("autosave")]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> AutoSave(
    [FromBody] Data.SimulationConfig model)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    await _configService.SaveAsync(model);
    return Ok();
  }

  // ---------------------------------
  // Spuštění simulace
  // ---------------------------------
  [HttpPost("run")]
  public async Task<IActionResult> Run()
  {
    try
    {
      var logs = await _simulationService.RunAsync(
        HttpContext.RequestAborted
      );

      return Ok(logs);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new[]
      {
        "ERROR: " + ex.Message
      });
    }
  }
}
