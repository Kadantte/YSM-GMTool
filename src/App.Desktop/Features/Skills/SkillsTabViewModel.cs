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

namespace App.Desktop.Features.Skills;

/// <summary>
/// Skills tab: learn / set / remove player and creature skills, self vs. selected player.
/// Self/other is decided by <see cref="IPlayerContext.Resolve"/> returning "self".
/// </summary>
public sealed class SkillsTabViewModel : TabModuleViewModel
{
    private const string SelfTarget = "self";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => "Skills";

    public override string IconKey => "fa-solid fa-wand-sparkles";

    public override int Order => 40;

    public EntityBrowserViewModel<SkillRecord> Browser { get; }

    public ReactiveCommand<Unit, Unit> LearnSkill { get; }

    public ReactiveCommand<Unit, Unit> SetSkillLevel { get; }

    public ReactiveCommand<Unit, Unit> RemoveSkill { get; }

    public ReactiveCommand<Unit, Unit> LearnAllSkill { get; }

    public ReactiveCommand<Unit, Unit> LearnCreatureSkill { get; }

    public ReactiveCommand<Unit, Unit> LearnCreatureAllSkill { get; }

    public SkillsTabViewModel(
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

        // Single icons-on snapshot: columns and row-values must share the same shape (#3).
        var iconsOn = Icons();

        Browser = new EntityBrowserViewModel<SkillRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<SkillRecord>("skills", ct)
                : repo.GetSkillsAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.SkillId,
            nameSelector: x => x.Skillname,
            rowValuesSelector: x => iconsOn
                ? new object?[] { x.IconFileName ?? string.Empty, x.SkillId, x.Skillname }
                : new object?[] { x.SkillId, x.Skillname },
            normalizer: norm,
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns = iconsOn
                ?
                [
                    new BrowserColumn("Icon", 44, IsImage: true, ImageSize: 36),
                    new BrowserColumn("ID", 80),
                    new BrowserColumn("Name", 460, Fill: true),
                ]
                :
                [
                    new BrowserColumn("ID", 80),
                    new BrowserColumn("Name", 460, Fill: true),
                ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Skills", ex.Message);

        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r => SkillId = r!.SkillId);

        LearnSkill = ReactiveCommand.CreateFromTask(() => SkillCommandAsync(
            self: id => LuaCommands.LearnSkill(id),
            other: (id, p) => LuaCommands.LearnSkillForPlayer(id, p)));

        SetSkillLevel = ReactiveCommand.CreateFromTask(() => SkillCommandAsync(
            self: id => LuaCommands.SetSkill(id, this.SkillLevel),
            other: (id, p) => LuaCommands.SetSkillForPlayer(id, this.SkillLevel, p)));

        RemoveSkill = ReactiveCommand.CreateFromTask(() => SkillCommandAsync(
            self: id => LuaCommands.RemoveSkill(id),
            other: (id, p) => LuaCommands.RemoveSkillForPlayer(id, p)));

        LearnAllSkill = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!_player.TryResolveRequired(out var p))
            {
                await _dlg.ShowWarningAsync("Skills", "Select player in the right sidebar for Learn all skill.");
                return;
            }

            await _cmd.DispatchAsync(LuaCommands.LearnAllSkill(p));
        });

        LearnCreatureSkill = ReactiveCommand.CreateFromTask(() => SkillCommandAsync(
            self: id => LuaCommands.LearnCreatureSkillSelf(id),
            other: (id, p) => LuaCommands.LearnCreatureSkill(id, p)));

        // No skillId, no guard: targets a creature slot directly.
        LearnCreatureAllSkill = ReactiveCommand.CreateFromTask(async () =>
        {
            var target = _player.Resolve();
            await _cmd.DispatchAsync(string.Equals(target, SelfTarget, StringComparison.OrdinalIgnoreCase)
                ? LuaCommands.LearnCreatureAllSkill(CreatureSlotIndex)
                : LuaCommands.LearnCreatureAllSkillForPlayer(CreatureSlotIndex, target));
        });

        // Auto-load game data at startup (failures are silent — see AutoLoadAsync).
        _ = Browser.AutoLoadAsync();
    }

    // --- Inputs. ---

    private int _skillId = 1;

    public int SkillId
    {
        get => _skillId;
        set => this.RaiseAndSetIfChanged(ref _skillId, value);
    }

    private int _skillLevel = 1;

    public int SkillLevel
    {
        get => _skillLevel;
        set => this.RaiseAndSetIfChanged(ref _skillLevel, value);
    }

    private int _creatureSlotIndex;

    public int CreatureSlotIndex
    {
        get => _creatureSlotIndex;
        set => this.RaiseAndSetIfChanged(ref _creatureSlotIndex, value);
    }

    private async Task SkillCommandAsync(Func<int, string> self, Func<int, string, string> other)
    {
        if (SkillId <= 0)
        {
            await _dlg.ShowWarningAsync("Skills", "Select skill or enter Skill ID first.");
            return;
        }

        var target = _player.Resolve();
        await _cmd.DispatchAsync(string.Equals(target, SelfTarget, StringComparison.OrdinalIgnoreCase)
            ? self(SkillId)
            : other(SkillId, target));
    }
}
