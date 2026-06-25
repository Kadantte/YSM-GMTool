namespace App.Desktop.Services;

/// <summary>The single funnel every generated Lua command flows through.</summary>
public interface ICommandDispatcher
{
    Task DispatchAsync(string luaCommand);
}
