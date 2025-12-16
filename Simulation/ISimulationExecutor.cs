namespace AxPlantSimWebApp.Simulation;

public interface ISimulationExecutor
{
  Task<List<string>> RunAsync(CancellationToken cancellationToken);
}
