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

namespace App.Desktop.Features.Summons;

/// <summary>
/// Summons tab: insert a summon (optionally at a stage) and enhance a creature slot's stage.
/// </summary>
public sealed class SummonsTabViewModel : TabModuleViewModel
{
    private readonly ICommandDispatcher _cmd;
    private readonly IDialogService _dlg;

    public override string Title => "Summons";

    public override string IconKey => "fa-solid fa-paw";

    public override int Order => 70;

    public EntityBrowserViewModel<SummonRecord> Browser { get; }

    public int[] Slots { get; } = [0, 1, 2, 3, 4, 5];

    public ReactiveCommand<Unit, Unit> AddSummon { get; }

    public ReactiveCommand<Unit, Unit> StageSummon { get; }

    public SummonsTabViewModel(
        IGameDataRepository repo,
        ILocalCacheService cache,
        INameNormalizer norm,
        IAppSettingsHolder settings,
        ConnectionStringResolver connection,
        ICommandDispatcher cmd,
        IDialogService dlg)
    {
        _cmd = cmd;
        _dlg = dlg;

        bool Icons() => settings.Current.EnableEntityIcons
            && !string.IsNullOrWhiteSpace(settings.Current.EntityIconsPath)
            && Directory.Exists(settings.Current.EntityIconsPath);

        // Single icons-on snapshot: columns and row-values must share the same shape (#3).
        var iconsOn = Icons();

        Browser = new EntityBrowserViewModel<SummonRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<SummonRecord>("summons", ct)
                : repo.GetSummonsAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.SummonId,
            nameSelector: x => x.SummonName,
            rowValuesSelector: x => iconsOn
                ? new object?[] { x.IconFileName ?? string.Empty, x.SummonId, x.SummonName, x.CardName ?? string.Empty }
                : new object?[] { x.SummonId, x.SummonName, x.CardName ?? string.Empty },
            normalizer: norm,
            searchableTextSelector: x => new[] { x.SummonName, x.CardName },
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns = iconsOn
                ?
                [
                    new BrowserColumn("Icon", 44, IsImage: true, ImageSize: 36),
                    new BrowserColumn("Summon ID", 100),
                    new BrowserColumn("Summon Name", 320, Fill: true),
                    new BrowserColumn("Card Name", 320, Fill: true),
                ]
                :
                [
                    new BrowserColumn("Summon ID", 100),
                    new BrowserColumn("Summon Name", 320, Fill: true),
                    new BrowserColumn("Card Name", 320, Fill: true),
                ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Summons", ex.Message);

        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r => SummonId = r!.SummonId);

        AddSummon = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SummonId <= 0)
            {
                await _dlg.ShowWarningAsync("Summons", "Select summon or enter Summon ID first.");
                return;
            }

            await _cmd.DispatchAsync(UseAddStage
                ? LuaCommands.InsertSummonByIdWithStage(SummonId, AddStage)
                : LuaCommands.InsertSummonById(SummonId));
        });

        StageSummon = ReactiveCommand.CreateFromTask(() =>
            _cmd.DispatchAsync(LuaCommands.StageSummon(Slot, Stage)));

        // Auto-load game data at startup (failures are silent — see AutoLoadAsync).
        _ = Browser.AutoLoadAsync();
    }

    // --- Inputs. ---

    private int _summonId;

    public int SummonId
    {
        get => _summonId;
        set => this.RaiseAndSetIfChanged(ref _summonId, value);
    }

    private bool _useAddStage;

    public bool UseAddStage
    {
        get => _useAddStage;
        set => this.RaiseAndSetIfChanged(ref _useAddStage, value);
    }

    private int _addStage;

    public int AddStage
    {
        get => _addStage;
        set => this.RaiseAndSetIfChanged(ref _addStage, value);
    }

    private int _slot;

    public int Slot
    {
        get => _slot;
        set => this.RaiseAndSetIfChanged(ref _slot, value);
    }

    private int _stage;

    public int Stage
    {
        get => _stage;
        set => this.RaiseAndSetIfChanged(ref _stage, value);
    }
}
