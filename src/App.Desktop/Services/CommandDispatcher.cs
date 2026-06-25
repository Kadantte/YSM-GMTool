using App.Core.Abstractions;
using App.Desktop.Infrastructure;

namespace App.Desktop.Services;

public sealed class CommandDispatcher(IClipboardService clipboard, ICommandHistoryService history, IAppSettingsHolder settings) : ICommandDispatcher
{
    public async Task DispatchAsync(string luaCommand)
    {
        var final = ApplyRunPrefix(luaCommand);
        history.Add(final);                 // <-- the fix: record every dispatched command
        await clipboard.SetTextAsync(final);
    }

    private string ApplyRunPrefix(string command)
    {
        if (!settings.Current.AppendGeneratedCommands)
        {
            return command;
        }

        var t = command.TrimStart();
        if (t.StartsWith("//", StringComparison.Ordinal) || t.StartsWith("/run ", StringComparison.OrdinalIgnoreCase))
        {
            return command;
        }

        return "/run " + command;
    }
}
