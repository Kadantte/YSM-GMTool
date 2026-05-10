using Microsoft.Data.Sqlite;

namespace App.Data.Infrastructure;

public static class SqliteSnapshotInitializer
{
    public const int SchemaVersion = 1;

    private const string CreateScript = """
        CREATE TABLE IF NOT EXISTS Items
            (item_id INTEGER PRIMARY KEY, name_en TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Monsters
            (id INTEGER PRIMARY KEY, name TEXT NOT NULL, level INTEGER, location TEXT);
        CREATE TABLE IF NOT EXISTS Npcs
            (npc_id INTEGER PRIMARY KEY, npc_title TEXT NOT NULL, x REAL, y REAL, contact_script TEXT);
        CREATE TABLE IF NOT EXISTS Skills
            (skill_id INTEGER PRIMARY KEY, skillname TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS States
            (state_id INTEGER PRIMARY KEY, buff_name TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Summons
            (summon_id INTEGER PRIMARY KEY, summon_name TEXT NOT NULL, card_name TEXT, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Meta
            (key TEXT PRIMARY KEY, value TEXT);

        CREATE INDEX IF NOT EXISTS idx_items_name    ON Items(name_en);
        CREATE INDEX IF NOT EXISTS idx_monsters_name ON Monsters(name);
        CREATE INDEX IF NOT EXISTS idx_npcs_title    ON Npcs(npc_title);
        CREATE INDEX IF NOT EXISTS idx_skills_name   ON Skills(skillname);
        CREATE INDEX IF NOT EXISTS idx_states_name   ON States(buff_name);
        CREATE INDEX IF NOT EXISTS idx_summons_name  ON Summons(summon_name);
        """;

    public static void Initialize(string filePath)
    {
        using var connection = new SqliteConnection($"Data Source={filePath}");
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = CreateScript;
        cmd.ExecuteNonQuery();
    }
}
