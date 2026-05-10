using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SqliteIconSourceTests
{
    [Fact]
    public void TryGetIconBytes_ReturnsBlobOrNull()
    {
        var dbPath = Path.GetTempFileName(); File.Delete(dbPath);
        try
        {
            using (var c = new SqliteConnection($"Data Source={dbPath}"))
            {
                c.Open();
                using var cmd = c.CreateCommand();
                cmd.CommandText = "CREATE TABLE Icons(file_name TEXT PRIMARY KEY, png BLOB NOT NULL); INSERT INTO Icons VALUES('x.png', x'010203');";
                cmd.ExecuteNonQuery();
            }
            using (var src = new SqliteIconSource(dbPath))
            {
                Assert.Equal(new byte[] { 1, 2, 3 }, src.TryGetIconBytes("x.png"));
                Assert.Null(src.TryGetIconBytes("missing.png"));
            }
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            if (File.Exists(dbPath)) File.Delete(dbPath);
        }
    }
}
