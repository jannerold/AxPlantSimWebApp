using AxPlantSimWebApp.Simulation;

namespace AxPlantSimWebApp.Services;

public class SimulationService
{
  private readonly ISimulationExecutor _executor;

  public SimulationService(ISimulationExecutor executor)
  {
    _executor = executor;
  }

  public Task<List<string>> RunAsync(CancellationToken cancellationToken)
  {
    return _executor.RunAsync(cancellationToken);
  }
}
