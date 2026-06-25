using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Models.Entities;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.ViewModels;
using ReactiveUI;

namespace App.Desktop.Features.Playerchecker;

/// <summary>
/// Playerchecker tab: live (never cached) character search with SQL-side filtering plus an
/// inventory/warehouse grid. Ported from the WinForms <c>PlayerCheckerActionsControl</c> +
/// <c>MainForm</c> playerchecker handlers.
/// </summary>
public sealed class PlayercheckerTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Playerchecker";

    private readonly IGameDataRepository _repo;
    private readonly ConnectionStringResolver _connection;
    private readonly IDialogService _dlg;

    private PlayerRecord? _selectedPlayer;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-users";

    public override int Order => 10;

    public EntityBrowserViewModel<PlayerRecord> Browser { get; }

    public ObservableCollection<InventoryItemRecord> Inventory { get; } = [];

    public ReactiveCommand<Unit, Unit> LoadAllCharacters { get; }

    public ReactiveCommand<Unit, Unit> LoadOnlineCharacters { get; }

    public ReactiveCommand<Unit, Unit> LoadInventory { get; }

    public ReactiveCommand<Unit, Unit> LoadWarehouse { get; }

    public ReactiveCommand<Unit, Unit> OpenInfos { get; }

    public PlayercheckerTabViewModel(
        IGameDataRepository repo,
        INameNormalizer norm,
        IAppSettingsHolder settings,
        ConnectionStringResolver connection,
        IDialogService dlg)
    {
        _repo = repo;
        _connection = connection;
        _dlg = dlg;

        Browser = new EntityBrowserViewModel<PlayerRecord>(
            loadAllAsync: ct => _repo.GetPlayersAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.PlayerId,
            nameSelector: x => x.PlayerName,
            rowValuesSelector: x => new object?[]
            {
                x.PlayerId,
                x.PlayerName,
                x.Account,
                x.Level,
                x.JobName ?? string.Empty,
                x.OnlineStatus,
            },
            normalizer: norm,
            sqlSearchAsync: (term, mode, ct) => _repo.GetCharactersBySearchAsync(
                connection.Provider,
                connection.Resolve(),
                term,
                searchByAccount: mode == SearchMode.ById,
                connection.Tokens(),
                ct),
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null,
            settingsHolder: settings)
        {
            Columns =
            [
                new BrowserColumn("ID", 80),
                new BrowserColumn("Name", 230, Fill: true),
                new BrowserColumn("Account", 200, Fill: true),
                new BrowserColumn("Level", 80),
                new BrowserColumn("Job", 160, Fill: true),
                new BrowserColumn("Status", 90),
            ],
            ByIdLabel = "by Account",
            ByNameLabel = "by Char Name",
            LoadAllVisible = false,
            RealtimeVisible = false,
            RealtimeEnabled = false,
            DebounceMs = 350,
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Data operation failed.", ex.Message);

        Browser.WhenSelectedRecordChanged.Subscribe(r => _selectedPlayer = r);

        LoadAllCharacters = ReactiveCommand.CreateFromTask(LoadAllCharactersAsync);
        LoadOnlineCharacters = ReactiveCommand.CreateFromTask(LoadOnlineCharactersAsync);
        LoadInventory = ReactiveCommand.CreateFromTask(LoadInventoryAsync);
        LoadWarehouse = ReactiveCommand.CreateFromTask(LoadWarehouseAsync);
        OpenInfos = ReactiveCommand.CreateFromTask(OpenInfosAsync);

        LoadAllCharacters.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load all characters.", ex.Message));
        LoadOnlineCharacters.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load online characters.", ex.Message));
        LoadInventory.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load inventory.", ex.Message));
        LoadWarehouse.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load warehouse.", ex.Message));
    }

    private string _inventoryTitle = string.Empty;

    public string InventoryTitle
    {
        get => _inventoryTitle;
        set => this.RaiseAndSetIfChanged(ref _inventoryTitle, value);
    }

    private async Task LoadAllCharactersAsync()
        => await Browser.LoadExternalAsync(
            ct => _repo.GetAllCharactersAsync(_connection.Provider, _connection.Resolve(), _connection.Tokens(), ct));

    private async Task LoadOnlineCharactersAsync()
        => await Browser.LoadExternalAsync(
            ct => _repo.GetOnlineCharactersAsync(_connection.Provider, _connection.Resolve(), _connection.Tokens(), ct));

    private async Task LoadInventoryAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Load Inventory", "Select a player first.");
            return;
        }

        var items = await _repo.GetInventoryAsync(
            _connection.Provider,
            _connection.Resolve(),
            _selectedPlayer.PlayerId,
            _connection.Tokens(),
            CancellationToken.None);

        PopulateInventory(items, $"Inventory — {_selectedPlayer.PlayerName} ({items.Count} item(s))");
    }

    private async Task LoadWarehouseAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Load Warehouse", "Select a player first.");
            return;
        }

        var items = await _repo.GetWarehouseAsync(
            _connection.Provider,
            _connection.Resolve(),
            _selectedPlayer.Account,
            _connection.Tokens(),
            CancellationToken.None);

        PopulateInventory(items, $"Warehouse — {_selectedPlayer.Account} ({items.Count} item(s))");
    }

    private async Task OpenInfosAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Player Info", "Select a player first.");
            return;
        }

        var r = _selectedPlayer;
        await _dlg.ShowInfoAsync(
            "Player Info",
            $"Name: {r.PlayerName}\nAccount: {r.Account}\nLevel: {r.Level}\nJob: {r.JobName}\nStatus: {r.OnlineStatus}");
    }

    private void PopulateInventory(System.Collections.Generic.IReadOnlyList<InventoryItemRecord> items, string title)
    {
        Inventory.Clear();
        foreach (var item in items)
        {
            Inventory.Add(item);
        }

        InventoryTitle = title;
    }
}
