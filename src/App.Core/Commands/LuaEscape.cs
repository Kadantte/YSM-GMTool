namespace App.Core.Commands;

public static class LuaEscape
{
    /// <summary>Escape for a Lua single-quoted string literal: '\' then '\''.</summary>
    public static string Single(string value)
        => (value ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("'", "\\'", StringComparison.Ordinal);

    /// <summary>Escape for a Lua double-quoted string literal: '\' then '\"'.</summary>
    public static string Double(string value)
        => (value ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal);
}
