using System.Data.SQLite;
using System.Text.Json;

namespace AxPlantSimWebApp.Services;

public class TableBrowserService
{
  private readonly string _connectionString;

  public TableBrowserService(IConfiguration config)
  {
    _connectionString = config.GetConnectionString("SQLite")
      ?? throw new InvalidOperationException("Chybí connection string 'SQLite'.");
  }

  // -------------------------------------------------------
  // Seznam tabulek
  // -------------------------------------------------------
  public List<string> GetTableNames()
  {
    var tables = new List<string>();

    using var conn = new SQLiteConnection(_connectionString);
    conn.Open();

    using var cmd = conn.CreateCommand();
    cmd.CommandText =
      @"SELECT name
          FROM sqlite_master
         WHERE type = 'table'
           AND name NOT LIKE 'sqlite_%'
         ORDER BY name;";

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
      tables.Add(reader.GetString(0));
    }

    return tables;
  }

  // -------------------------------------------------------
  // Načtení dat z tabulky
  // -------------------------------------------------------
  public TableViewModel GetTable(string tableName, int limit = 100)
  {
    var model = new TableViewModel
    {
      TableName = tableName,
      Columns = new List<string>(),
      Rows = new List<Dictionary<string, object?>>()
    };

    using var conn = new SQLiteConnection(_connectionString);
    conn.Open();

    // sloupce
    using (var pragma = conn.CreateCommand())
    {
      pragma.CommandText = $"PRAGMA table_info('{tableName}');";
      using var r = pragma.ExecuteReader();
      while (r.Read())
      {
        var colName = r.GetString(1); // name
        model.Columns.Add(colName);
      }
    }

    if (!model.Columns.Any())
      return model;

    // data
    using (var cmd = conn.CreateCommand())
    {
      cmd.CommandText = $"SELECT * FROM \"{tableName}\" LIMIT @limit;";
      cmd.Parameters.AddWithValue("@limit", limit);

      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        var row = new Dictionary<string, object?>();

        foreach (var col in model.Columns)
        {
          object? v = reader[col];

          if (v == DBNull.Value)
          {
            row[col] = null;
          }
          else
          {
            row[col] = CleanValue(v);
          }
        }

        model.Rows.Add(row);
      }
    }

    return model;
  }

  // -------------------------------------------------------
  // Update řádku (volaný z inline editace)
  // -------------------------------------------------------
  public void UpdateRow(string tableName, Dictionary<string, JsonElement> row)
  {
    using var conn = new SQLiteConnection(_connectionString);
    conn.Open();

    if (!row.TryGetValue("Id", out var idElement))
      throw new Exception("Row must contain primary key column 'Id'.");

    var idValue = ConvertJsonElement(idElement);

    var columns = row.Keys.Where(k => k != "Id").ToList();
    if (!columns.Any())
      return;

    string setClause = string.Join(", ", columns.Select(c => $"{c} = @{c}"));

    using var cmd = conn.CreateCommand();
    cmd.CommandText = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

    foreach (var col in columns)
    {
      var value = ConvertJsonElement(row[col]);
      cmd.Parameters.AddWithValue($"@{col}", value ?? DBNull.Value);
    }

    cmd.Parameters.AddWithValue("@Id", idValue ?? DBNull.Value);

    var count = cmd.ExecuteNonQuery();
    Console.WriteLine($"UPDATE {tableName} Id={idValue}, rows affected = {count}");
  }

  // SQLite → normální .NET typy
  private object? CleanValue(object? value)
  {
    if (value == null || value == DBNull.Value)
      return null;

    return value switch
    {
      long v => v,
      int v => v,
      double v => v,
      float v => v,
      bool v => v,
      string v => v,
      byte[] bytes => System.Text.Encoding.UTF8.GetString(bytes),
      DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
      _ => value.ToString()
    };
  }

  // JsonElement → .NET typy
  private object? ConvertJsonElement(JsonElement element)
  {
    switch (element.ValueKind)
    {
      case JsonValueKind.Null:
      case JsonValueKind.Undefined:
        return null;

      case JsonValueKind.String:
        return element.GetString();

      case JsonValueKind.Number:
        if (element.TryGetInt64(out var l))
          return l;
        if (element.TryGetDouble(out var d))
          return d;
        return null;

      case JsonValueKind.True:
        return true;

      case JsonValueKind.False:
        return false;

      default:
        return element.ToString();
    }
  }
}

// ViewModel pro tabulku
public class TableViewModel
{
  public string TableName { get; set; } = "";
  public List<string> Columns { get; set; } = new();
  public List<Dictionary<string, object?>> Rows { get; set; } = new();
}
