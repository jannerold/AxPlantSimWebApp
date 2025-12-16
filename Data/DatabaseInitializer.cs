using Microsoft.Data.Sqlite;

namespace AxPlantSimWebApp.Data;

public static class DatabaseInitializer
{
  public static void EnsureImportRunTable(string connectionString)
  {
    using var conn = new SqliteConnection(connectionString);
    conn.Open();

    using var cmd = conn.CreateCommand();
    cmd.CommandText =
      """
      CREATE TABLE IF NOT EXISTS import_runs (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        created_at TEXT NOT NULL
      );
      """;

    cmd.ExecuteNonQuery();
  }
}
