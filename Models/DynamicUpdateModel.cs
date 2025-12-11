using System.Text.Json;

namespace AxPlantSimWebApp.Models;

public class DynamicUpdateModel
{
  public string TableName { get; set; } = "";
  public Dictionary<string, JsonElement> Row { get; set; } = new();
}
