using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SqliteSnapshotInitializerTests
{
    [Fact]
    public void Initialize_CreatesAllExpectedTablesAndIndexes()
    {
        var path = Path.GetTempFileName();
        File.Delete(path);
        try
        {
            SqliteSnapshotInitializer.Initialize(path);

            var names = new List<string>();
            using (var conn = new SqliteConnection($"Data Source={path}"))
            {
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type IN ('table','index') ORDER BY name";
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) names.Add(reader.GetString(0));
                cmd.Dispose();
            }

            Assert.Contains("Items", names);
            Assert.Contains("Monsters", names);
            Assert.Contains("Npcs", names);
            Assert.Contains("Skills", names);
            Assert.Contains("States", names);
            Assert.Contains("Summons", names);
            Assert.Contains("Meta", names);
            Assert.Contains("idx_items_name", names);
            Assert.Contains("idx_monsters_name", names);
            Assert.Contains("idx_npcs_title", names);
            Assert.Contains("idx_skills_name", names);
            Assert.Contains("idx_states_name", names);
            Assert.Contains("idx_summons_name", names);
        }
        finally
        {
            var clearConn = new SqliteConnection($"Data Source={path}");
            SqliteConnection.ClearPool(clearConn);
            System.Threading.Thread.Sleep(100);
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
