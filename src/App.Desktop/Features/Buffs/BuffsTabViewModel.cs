using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Commands;
using App.Core.Models.Entities;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.ViewModels;
using ReactiveUI;

namespace App.Desktop.Features.Buffs;

/// <summary>
/// Buffs / states tab: world &amp; event states (no player), plus player/creature buffs that require a
/// selected player. <see cref="IsSummonTarget"/> toggles between player (add_state) and creature (add_cstate).
/// </summary>
public sealed class BuffsTabViewModel : TabModuleViewModel
{
    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => "Buffs";

    public override string IconKey => "fa-solid fa-bolt";

    public override int Order => 50;

    public EntityBrowserViewModel<StateRecord> Browser { get; }

    public ReactiveCommand<Unit, Unit> AddTimedWorldState { get; }

    public ReactiveCommand<Unit, Unit> AddEventState { get; }

    public ReactiveCommand<Unit, Unit> RemoveEventState { get; }

    public ReactiveCommand<Unit, Unit> AddBuff { get; }

    public ReactiveCommand<Unit, Unit> RemoveBuff { get; }

    public BuffsTabViewModel(
        IGameDataRepository repo,
        ILocalCacheService cache,
        INameNormalizer norm,
        IAppSettingsHolder settings,
        ConnectionStringResolver connection,
        ICommandDispatcher cmd,
        IPlayerContext player,
        IDialogService dlg)
    {
        _cmd = cmd;
        _player = player;
        _dlg = dlg;

        bool Icons() => settings.Current.EnableEntityIcons
            && !string.IsNullOrWhiteSpace(settings.Current.EntityIconsPath)
            && Directory.Exists(settings.Current.EntityIconsPath);

        Browser = new EntityBrowserViewModel<StateRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<StateRecord>("states", ct)
                : repo.GetStatesAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.StateId,
            nameSelector: x => x.BuffName,
            rowValuesSelector: x => Icons()
                ? new object?[] { x.IconFileName ?? string.Empty, x.StateId, x.BuffName }
                : new object?[] { x.StateId, x.BuffName },
            normalizer: norm,
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns = Icons()
                ?
                [
                    new BrowserColumn("Icon", 28, IsImage: true, ImageSize: 20),
                    new BrowserColumn("State ID", 100),
                    new BrowserColumn("Buff name", 460, Fill: true),
                ]
                :
                [
                    new BrowserColumn("State ID", 100),
                    new BrowserColumn("Buff name", 460, Fill: true),
                ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Buffs", ex.Message);

        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r =>
            {
                StateId = r!.StateId;
                BuffName = r.BuffName;
            });

        AddTimedWorldState = ReactiveCommand.CreateFromTask(() => StateCommandAsync(
            () => LuaCommands.CastWorldState(StateId, BuffLevel, DurationMinutes)));

        AddEventState = ReactiveCommand.CreateFromTask(() => StateCommandAsync(
            () => LuaCommands.AddEventState(StateId, BuffLevel)));

        RemoveEventState = ReactiveCommand.CreateFromTask(() => StateCommandAsync(
            () => LuaCommands.RemoveEventState(StateId)));

        AddBuff = ReactiveCommand.CreateFromTask(() => BuffCommandAsync(
            player: p => IsSummonTarget
                ? LuaCommands.AddCreatureState(StateId, BuffLevel, DurationMinutes, p)
                : LuaCommands.AddPlayerState(StateId, BuffLevel, DurationMinutes, p)));

        RemoveBuff = ReactiveCommand.CreateFromTask(() => BuffCommandAsync(
            player: p => IsSummonTarget
                ? LuaCommands.RemoveCreatureState(StateId, p)
                : LuaCommands.RemovePlayerState(StateId, p)));
    }

    // --- Inputs. ---

    private int _stateId = 1;

    public int StateId
    {
        get => _stateId;
        set => this.RaiseAndSetIfChanged(ref _stateId, value);
    }

    private string _buffName = string.Empty;

    public string BuffName
    {
        get => _buffName;
        set => this.RaiseAndSetIfChanged(ref _buffName, value);
    }

    private int _buffLevel = 1;

    public int BuffLevel
    {
        get => _buffLevel;
        set => this.RaiseAndSetIfChanged(ref _buffLevel, value);
    }

    private int _durationMinutes = 1;

    public int DurationMinutes
    {
        get => _durationMinutes;
        set => this.RaiseAndSetIfChanged(ref _durationMinutes, value);
    }

    private bool _isSummonTarget;

    public bool IsSummonTarget
    {
        get => _isSummonTarget;
        set => this.RaiseAndSetIfChanged(ref _isSummonTarget, value);
    }

    private async Task<bool> GuardStateAsync()
    {
        if (StateId <= 0)
        {
            await _dlg.ShowWarningAsync("Buffs", "Select buff/state or enter Buff-ID first.");
            return false;
        }

        return true;
    }

    private async Task StateCommandAsync(Func<string> build)
    {
        if (!await GuardStateAsync())
        {
            return;
        }

        await _cmd.DispatchAsync(build());
    }

    private async Task BuffCommandAsync(Func<string, string> player)
    {
        if (!await GuardStateAsync())
        {
            return;
        }

        if (!_player.TryResolveRequired(out var p))
        {
            await _dlg.ShowWarningAsync("Buffs", "Select player in the right sidebar.");
            return;
        }

        await _cmd.DispatchAsync(player(p));
    }
}
