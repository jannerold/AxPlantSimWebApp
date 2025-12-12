using Microsoft.Extensions.Configuration;

namespace AxPlantSimWebApp.Services;

public class ColumnNameMapper
{
  private readonly Dictionary<string, Dictionary<string, string>> _map;
  private readonly Dictionary<string, string> _tableNames;

  public ColumnNameMapper(IConfiguration config)
  {
    // --- ColumnNames ---
    var rootSection = config.GetSection("ColumnNames");
    var raw = rootSection.Get<Dictionary<string, Dictionary<string, string>>>();

    _map = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

    if (raw != null)
    {
      foreach (var tableEntry in raw)
      {
        var tableName = tableEntry.Key;
        var rawColumns = tableEntry.Value ?? new Dictionary<string, string>();

        var columnDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var colEntry in rawColumns)
        {
          columnDict[colEntry.Key] = colEntry.Value;
        }

        _map[tableName] = columnDict;
      }
    }

    // --- TableNames ---
    var tableSection = config.GetSection("TableNames");
    var rawTables = tableSection.Get<Dictionary<string, string>>();

    _tableNames = rawTables != null
      ? new Dictionary<string, string>(rawTables, StringComparer.OrdinalIgnoreCase)
      : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
  }

  // sloupce
  public string GetDisplayName(string table, string column)
  {
    if (string.IsNullOrWhiteSpace(table) || string.IsNullOrWhiteSpace(column))
      return column ?? string.Empty;

    if (_map.TryGetValue(table, out var columns) &&
        columns.TryGetValue(column, out var translated))
    {
      return translated;
    }

    return column;
  }

  public string GetAxisLabel(string table, string column)
  {
    return GetDisplayName(table, column);
  }

  // názvy tabulek
  public string GetTableDisplayName(string table)
  {
    if (string.IsNullOrWhiteSpace(table))
      return table ?? string.Empty;

    if (_tableNames.TryGetValue(table, out var translated))
    {
      return translated;
    }

    return table;
  }
}
