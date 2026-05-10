using App.Data.Services;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class IconPackServiceTests
{
    [Fact]
    public async Task PackAsync_StoresAllPngsKeyedByFileName()
    {
        var srcDir = Path.Combine(Path.GetTempPath(), "icons_" + Guid.NewGuid().ToString("N"));
        var dbPath = Path.GetTempFileName(); File.Delete(dbPath);
        Directory.CreateDirectory(srcDir);
        try
        {
            var aBytes = new byte[] { 1, 2, 3 };
            var bBytes = new byte[] { 4, 5 };
            File.WriteAllBytes(Path.Combine(srcDir, "a.png"), aBytes);
            File.WriteAllBytes(Path.Combine(srcDir, "b.png"), bBytes);

            var service = new IconPackService();
            var packed = await service.PackAsync(srcDir, dbPath, null, CancellationToken.None);

            Assert.Equal(2, packed);

            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT png FROM Icons WHERE file_name=$n";
            cmd.Parameters.AddWithValue("$n", "a.png");
            Assert.Equal(aBytes, (byte[])cmd.ExecuteScalar()!);
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            if (Directory.Exists(srcDir)) Directory.Delete(srcDir, true);
            if (File.Exists(dbPath)) File.Delete(dbPath);
        }
    }
}
