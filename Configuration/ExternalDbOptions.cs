namespace AxPlantSimWebApp.Configuration;

public sealed class ExternalDbOptions
{
  public string Provider { get; set; } = "SqlServer";
  public string ConnectionString { get; set; } = "";
  public int KeepRuns { get; set; } = 3;
  public List<ExternalTableMap> Tables { get; set; } = new();
}

public sealed class ExternalTableMap
{
  public string SourceTable { get; set; } = "";
  public string TargetTable { get; set; } = "";
}
