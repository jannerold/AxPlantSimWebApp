using System.Net.Http.Json;

namespace AxPlantSimWebApp.Simulation;

public class AgentSimulationExecutor : ISimulationExecutor
{
  private readonly HttpClient _http;

  public AgentSimulationExecutor(HttpClient http)
  {
    _http = http;
  }

  public async Task<List<string>> RunAsync(CancellationToken cancellationToken)
  {
    // volání lokálního simulačního agenta
    var response = await _http.PostAsync(
      "run",
      content: null,
      cancellationToken
    );

    response.EnsureSuccessStatusCode();

    var lines = await response.Content
      .ReadFromJsonAsync<List<string>>(cancellationToken: cancellationToken);

    return lines ?? new List<string>
    {
      "Simulační agent nevrátil žádná data."
    };
  }
}
