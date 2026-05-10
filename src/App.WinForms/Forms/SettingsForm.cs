using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Models;

namespace App.WinForms.Forms;

public partial class SettingsForm : Form
{
    private static readonly (string Key, string Label)[] CacheEntities =
    [
        ("monsters",  "Monsters"),
        ("items",     "Items"),
        ("skills",    "Skills"),
        ("states",    "Buffs/States"),
        ("npcs",      "NPCs"),
        ("summons",   "Summons"),
    ];

    private readonly IGameDataRepository _repository;
    private readonly IConnectionStringBuilderService _connectionStringBuilder;
    private readonly ILocalCacheService _localCacheService;
    private readonly ISnapshotExportService _exportService;
    private readonly IIconPackService _iconPackService;
    private readonly AppSettings _workingSettings;
    private readonly AppMode _mode;
    private bool _isLoading;
    private CheckBox? _chkEnableEntityIcons;
    private TextBox? _txtEntityIconsPath;
    private Button? _btnBrowseEntityIconsPath;
    private Label? _lblIconsPath;
    private CheckBox? _chkForceLiveMode;
    private Label? _lblForceLiveModeHint;

    public SettingsForm(
        IGameDataRepository repository,
        IConnectionStringBuilderService connectionStringBuilder,
        ILocalCacheService localCacheService,
        AppSettings currentSettings,
        AppMode mode,
        ISnapshotExportService exportService,
        IIconPackService iconPackService)
    {
        _repository = repository;
        _connectionStringBuilder = connectionStringBuilder;
        _localCacheService = localCacheService;
        _exportService = exportService;
        _iconPackService = iconPackService;
        _workingSettings = currentSettings.Clone();
        _mode = mode;

        InitializeComponent();
        InitializeIconSettingsSection();
        InitializeForceLiveModeSection();
        InitializeExportSnapshotSection();
        ApplyDialogIcon();
        LoadSettingsIntoControls();
        RefreshCacheStatus();
        ApplyOfflineModeUi();
    }

    private void ApplyOfflineModeUi()
    {
        if (_mode != AppMode.OfflineSnapshot) return;

        // Connection and Table Names tabs remain visible so the user can configure
        // a live DB and re-enable live mode via the "Force Live mode" checkbox.
        if (_lblIconsPath is not null) _lblIconsPath.Visible = false;
        if (_txtEntityIconsPath is not null) _txtEntityIconsPath.Visible = false;
        if (_btnBrowseEntityIconsPath is not null) _btnBrowseEntityIconsPath.Visible = false;
    }

    public AppSettings UpdatedSettings => _workingSettings.Clone();

    private void LoadSettingsIntoControls()
    {
        _isLoading = true;

        cmbProvider.DataSource = Enum.GetValues<DatabaseProvider>();
        cmbProvider.SelectedItem = _workingSettings.Provider;

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Server)
            && !string.IsNullOrWhiteSpace(_workingSettings.ConnectionString)
            && _connectionStringBuilder.TryParse(_workingSettings.Provider, _workingSettings.ConnectionString, out var parsed))
        {
            _workingSettings.Connection = parsed;
        }

        txtServer.Text = _workingSettings.Connection.Server;
        var port = _workingSettings.Connection.Port > 0
            ? _workingSettings.Connection.Port
            : (_workingSettings.Provider == DatabaseProvider.MSSQL ? 1433 : 3306);
        nudPort.Value = Math.Clamp(port, (int)nudPort.Minimum, (int)nudPort.Maximum);
        txtDatabase.Text = _workingSettings.Connection.Database;
        txtUserId.Text = _workingSettings.Connection.UserId;
        txtPassword.Text = _workingSettings.Connection.Password;
        chkIntegratedSecurity.Checked = _workingSettings.Connection.IntegratedSecurity;
        chkEncrypt.Checked = _workingSettings.Connection.Encrypt;
        chkTrustServerCertificate.Checked = _workingSettings.Connection.TrustServerCertificate;

        txtArcadiaName.Text = _workingSettings.TableNames.ArcadiaName;
        txtTelecasterName.Text = _workingSettings.TableNames.TelecasterName;
        txtAuthName.Text = _workingSettings.TableNames.AuthName;
        txtAccountsName.Text = _workingSettings.TableNames.AccountsName;
        txtCharacterResource.Text = _workingSettings.TableNames.CharacterResource;
        txtMonsterResource.Text = _workingSettings.TableNames.MonsterResource;
        txtStringResource.Text = _workingSettings.TableNames.StringResource;
        txtItemResource.Text = _workingSettings.TableNames.ItemResource;
        txtSkillResource.Text = _workingSettings.TableNames.SkillResource;
        txtStateResource.Text = _workingSettings.TableNames.StateResource;
        txtNpcResource.Text = _workingSettings.TableNames.NpcResource;
        txtSummonResource.Text = _workingSettings.TableNames.SummonResource;

        chkLimitSelectQueries.Checked = _workingSettings.LimitSelectQueries;
        chkUseLocalCache.Checked = _workingSettings.UseLocalCache;
        if (_chkEnableEntityIcons is not null)
        {
            _chkEnableEntityIcons.Checked = _workingSettings.EnableEntityIcons;
        }

        if (_txtEntityIconsPath is not null)
        {
            _txtEntityIconsPath.Text = _workingSettings.EntityIconsPath ?? string.Empty;
        }

        if (_chkForceLiveMode is not null)
        {
            _chkForceLiveMode.Checked = _workingSettings.ForceLiveMode;
        }

        _isLoading = false;

        UpdateAuthUi();
        UpdateIconSettingsUi();
    }

    private void RefreshCacheStatus()
    {
        var dates = CacheEntities
            .Select(e => _localCacheService.GetCacheDate(e.Key))
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .ToList();

        if (dates.Count == 0)
        {
            lblCacheStatus.Text = "No cache found.";
        }
        else
        {
            var oldest = dates.Min();
            lblCacheStatus.Text = $"Cache available — last export: {oldest:yyyy-MM-dd HH:mm} ({dates.Count}/{CacheEntities.Length} tables)";
        }
    }

    private void ReadControlsIntoWorkingSettings()
    {
        _workingSettings.Provider = cmbProvider.SelectedItem is DatabaseProvider provider
            ? provider
            : DatabaseProvider.MSSQL;

        _workingSettings.Connection.Server = txtServer.Text.Trim();
        _workingSettings.Connection.Port = (int)nudPort.Value;
        _workingSettings.Connection.Database = txtDatabase.Text.Trim();
        _workingSettings.Connection.UserId = txtUserId.Text.Trim();
        _workingSettings.Connection.Password = txtPassword.Text;
        _workingSettings.Connection.IntegratedSecurity = chkIntegratedSecurity.Checked;
        _workingSettings.Connection.Encrypt = chkEncrypt.Checked;
        _workingSettings.Connection.TrustServerCertificate = chkTrustServerCertificate.Checked;

        _workingSettings.TableNames.ArcadiaName = txtArcadiaName.Text.Trim();
        _workingSettings.TableNames.TelecasterName = txtTelecasterName.Text.Trim();
        _workingSettings.TableNames.AuthName = txtAuthName.Text.Trim();
        _workingSettings.TableNames.AccountsName = txtAccountsName.Text.Trim();
        _workingSettings.TableNames.CharacterResource = txtCharacterResource.Text.Trim();
        _workingSettings.TableNames.MonsterResource = txtMonsterResource.Text.Trim();
        _workingSettings.TableNames.StringResource = txtStringResource.Text.Trim();
        _workingSettings.TableNames.ItemResource = txtItemResource.Text.Trim();
        _workingSettings.TableNames.SkillResource = txtSkillResource.Text.Trim();
        _workingSettings.TableNames.StateResource = txtStateResource.Text.Trim();
        _workingSettings.TableNames.NpcResource = txtNpcResource.Text.Trim();
        _workingSettings.TableNames.SummonResource = txtSummonResource.Text.Trim();

        _workingSettings.LimitSelectQueries = chkLimitSelectQueries.Checked;
        _workingSettings.UseLocalCache = chkUseLocalCache.Checked;
        _workingSettings.EnableEntityIcons = _chkEnableEntityIcons?.Checked == true;
        _workingSettings.EntityIconsPath = _txtEntityIconsPath?.Text.Trim() ?? string.Empty;
        _workingSettings.ForceLiveMode = _chkForceLiveMode?.Checked == true;

        _workingSettings.ConnectionString = _connectionStringBuilder.Build(_workingSettings.Provider, _workingSettings.Connection);
    }

    private void ApplyDialogIcon()
    {
        try
        {
            using var exeIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (exeIcon is not null)
            {
                Icon = (Icon)exeIcon.Clone();
                ShowIcon = true;
            }
        }
        catch
        {
            // Keep default if extraction fails.
        }
    }

    private bool TryBuildConnectionString(out string connectionString, out string validationMessage)
    {
        connectionString = string.Empty;
        validationMessage = string.Empty;

        ReadControlsIntoWorkingSettings();

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Server))
        {
            validationMessage = "Server is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Database))
        {
            validationMessage = "Database is required.";
            return false;
        }

        if (_workingSettings.Provider == DatabaseProvider.MSSQL && !_workingSettings.Connection.IntegratedSecurity)
        {
            if (string.IsNullOrWhiteSpace(_workingSettings.Connection.UserId))
            {
                validationMessage = "User ID is required for SQL authentication.";
                return false;
            }
        }

        if (_workingSettings.Provider == DatabaseProvider.MySQL && string.IsNullOrWhiteSpace(_workingSettings.Connection.UserId))
        {
            validationMessage = "User ID is required for MySQL.";
            return false;
        }

        if (_workingSettings.EnableEntityIcons)
        {
            if (string.IsNullOrWhiteSpace(_workingSettings.EntityIconsPath))
            {
                validationMessage = "Icons folder is required when entity icons are enabled.";
                return false;
            }

            if (!Directory.Exists(_workingSettings.EntityIconsPath))
            {
                validationMessage = "Selected icons folder does not exist.";
                return false;
            }
        }

        connectionString = _connectionStringBuilder.Build(_workingSettings.Provider, _workingSettings.Connection);
        return true;
    }

    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTestConnection.Enabled = false;
        lblStatus.Text = "Testing connection...";

        try
        {
            await _repository.TestConnectionAsync(_workingSettings.Provider, connectionString);
            lblStatus.Text = "Connection successful.";
            MessageBox.Show(this, "Connection successful.", "Database Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Connection test failed.";
            MessageBox.Show(this, ex.Message, "Database Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnTestConnection.Enabled = true;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        TrySaveToEnv(connectionString);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void TrySaveToEnv(string connectionString)
    {
        try
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var envPath = Path.Combine(appDirectory, ".env");

            File.WriteAllLines(envPath, new[]
            {
                $"YSM_DB_PROVIDER={_workingSettings.Provider}",
                $"YSM_DB_CONNECTION_STRING={connectionString}"
            });
        }
        catch
        {
            // .env save is best-effort; don't block saving settings.
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void btnExportCache_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnExportCache.Enabled = false;
        var queryTokens = _workingSettings.TableNames.ToTokenMap();

        try
        {
            lblStatus.Text = "Exporting Monsters...";
            var monsters = await _repository.GetMonstersAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("monsters", monsters);

            lblStatus.Text = "Exporting Items...";
            var items = await _repository.GetItemsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("items", items);

            lblStatus.Text = "Exporting Skills...";
            var skills = await _repository.GetSkillsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("skills", skills);

            lblStatus.Text = "Exporting Buffs/States...";
            var states = await _repository.GetStatesAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("states", states);

            lblStatus.Text = "Exporting NPCs...";
            var npcs = await _repository.GetNpcsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("npcs", npcs);

            lblStatus.Text = "Exporting Summons...";
            var summons = await _repository.GetSummonsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("summons", summons);

            RefreshCacheStatus();
            lblStatus.Text = "Cache export complete.";
            MessageBox.Show(this, "All tables exported to local cache successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Cache export failed.";
            MessageBox.Show(this, ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnExportCache.Enabled = true;
        }
    }

    private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        if (cmbProvider.SelectedItem is not DatabaseProvider provider)
        {
            return;
        }

        if (provider == DatabaseProvider.MSSQL && nudPort.Value == 3306)
        {
            nudPort.Value = 1433;
        }
        else if (provider == DatabaseProvider.MySQL && nudPort.Value == 1433)
        {
            nudPort.Value = 3306;
        }

        UpdateAuthUi();
    }

    private void ConnectionField_Changed(object? sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        UpdateAuthUi();
        UpdateIconSettingsUi();
    }

    private void UpdateAuthUi()
    {
        var isMsSql = cmbProvider.SelectedItem is DatabaseProvider.MSSQL;
        chkIntegratedSecurity.Enabled = isMsSql;
        chkEncrypt.Enabled = isMsSql;
        chkTrustServerCertificate.Enabled = isMsSql;

        var useSqlAuth = !isMsSql || !chkIntegratedSecurity.Checked;
        txtUserId.Enabled = useSqlAuth;
        txtPassword.Enabled = useSqlAuth;
    }

    private void InitializeIconSettingsSection()
    {
        _chkEnableEntityIcons = new CheckBox
        {
            AutoSize = true,
            Name = "chkEnableEntityIcons",
            Text = "Enable entity icons (Items, Skills, Buffs, Summons)",
            Margin = new Padding(3, 3, 3, 3)
        };
        _chkEnableEntityIcons.CheckedChanged += ConnectionField_Changed;

        _txtEntityIconsPath = new TextBox
        {
            Dock = DockStyle.Fill,
            Name = "txtEntityIconsPath",
            PlaceholderText = "Select icons directory..."
        };
        _txtEntityIconsPath.TextChanged += ConnectionField_Changed;

        _btnBrowseEntityIconsPath = new Button
        {
            AutoSize = true,
            Name = "btnBrowseEntityIconsPath",
            Text = "Browse..."
        };
        _btnBrowseEntityIconsPath.Click += btnBrowseEntityIconsPath_Click;

        var iconPathRow = new TableLayoutPanel
        {
            Name = "tlpEntityIconPath",
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 3,
            Margin = new Padding(0, 0, 0, 0)
        };
        iconPathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        iconPathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        iconPathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));

        _lblIconsPath = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Text = "Icons folder"
        };

        iconPathRow.Controls.Add(_lblIconsPath, 0, 0);
        iconPathRow.Controls.Add(_txtEntityIconsPath, 1, 0);
        iconPathRow.Controls.Add(_btnBrowseEntityIconsPath, 2, 0);

        tlpGeneral.SuspendLayout();
        tlpGeneral.Controls.Remove(tlpCacheRow);
        tlpGeneral.RowStyles.Clear();
        tlpGeneral.RowCount = 6;
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpGeneral.Controls.Add(_chkEnableEntityIcons, 0, 2);
        tlpGeneral.Controls.Add(iconPathRow, 0, 3);
        tlpGeneral.Controls.Add(tlpCacheRow, 0, 4);
        tlpGeneral.ResumeLayout();
    }

    private void InitializeForceLiveModeSection()
    {
        _chkForceLiveMode = new CheckBox
        {
            AutoSize = true,
            Name = "chkForceLiveMode",
            Text = "Force Live mode (ignore snapshot file)",
            Margin = new Padding(3, 8, 3, 0)
        };
        _chkForceLiveMode.CheckedChanged += (_, _) =>
        {
            if (_isLoading) return;
            _workingSettings.ForceLiveMode = _chkForceLiveMode!.Checked;
        };

        var snapshotPath = Path.Combine(AppContext.BaseDirectory, "gmtool-snapshot.db");
        var snapshotDescriptor = File.Exists(snapshotPath) ? snapshotPath : "none";

        _lblForceLiveModeHint = new Label
        {
            AutoSize = true,
            Name = "lblForceLiveModeHint",
            Text = $"Takes effect after restart. Snapshot file: {snapshotDescriptor}",
            Margin = new Padding(20, 0, 3, 8),
            ForeColor = SystemColors.GrayText
        };

        tlpGeneral.SuspendLayout();
        var checkboxRowIndex = tlpGeneral.RowCount;
        var hintRowIndex = checkboxRowIndex + 1;
        tlpGeneral.RowCount = hintRowIndex + 1;
        if (tlpGeneral.RowStyles.Count > 0)
        {
            tlpGeneral.RowStyles.RemoveAt(tlpGeneral.RowStyles.Count - 1);
        }
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpGeneral.Controls.Add(_chkForceLiveMode, 0, checkboxRowIndex - 1);
        tlpGeneral.Controls.Add(_lblForceLiveModeHint, 0, hintRowIndex - 1);
        tlpGeneral.ResumeLayout();
    }

    private void InitializeExportSnapshotSection()
    {
        if (_mode == AppMode.OfflineSnapshot) return;

        var btnExportSnapshot = new Button
        {
            Text = "Export Database to Snapshot…",
            Name = "btnExportSnapshot",
            AutoSize = false,
            Size = new Size(220, 28),
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
            Margin = new Padding(3)
        };
        btnExportSnapshot.Click += async (s, e) => await BtnExportSnapshot_ClickAsync();

        tlpGeneral.SuspendLayout();
        var newRowIndex = tlpGeneral.RowCount;
        tlpGeneral.RowCount = newRowIndex + 1;
        // Move the percent-fill row down: remove last style (which is the percent fill at index newRowIndex - 1)
        // and re-add at the end so the button row sits above it.
        if (tlpGeneral.RowStyles.Count > 0)
        {
            tlpGeneral.RowStyles.RemoveAt(tlpGeneral.RowStyles.Count - 1);
        }
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpGeneral.Controls.Add(btnExportSnapshot, 0, newRowIndex - 1);
        tlpGeneral.ResumeLayout();
    }

    private async Task BtnExportSnapshot_ClickAsync()
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Save Snapshot",
            Filter = "GM Tool Snapshot (*.db)|*.db",
            FileName = "gmtool-snapshot.db",
            InitialDirectory = AppContext.BaseDirectory
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        using var progressForm = new ExportProgressForm();
        var progress = new Progress<SnapshotExportProgress>(p => progressForm.Report(p.EntityName, p.Current, p.Total));

        var connectionString = _connectionStringBuilder.Build(_workingSettings.Provider, _workingSettings.Connection);
        var tokens = _workingSettings.TableNames.ToTokenMap();

        var task = _exportService.ExportAsync(
            _workingSettings.Provider,
            connectionString,
            tokens,
            dlg.FileName,
            progress,
            CancellationToken.None);

        progressForm.Show(this);
        try
        {
            await task;
            MessageBox.Show(this, "Snapshot exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Snapshot export failed");
            MessageBox.Show(this, $"Export failed: {ex.Message}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        finally
        {
            progressForm.Close();
        }

        if (MessageBox.Show(this, "Also pack icons from the configured Entity Icons Path?", "Pack Icons",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        if (string.IsNullOrWhiteSpace(_workingSettings.EntityIconsPath) ||
            !Directory.Exists(_workingSettings.EntityIconsPath))
        {
            MessageBox.Show(this, "Entity Icons Path is not configured or does not exist.", "Pack Icons",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var iconsDb = Path.Combine(Path.GetDirectoryName(dlg.FileName)!, "gmtool-icons.db");
        using var iconProgressForm = new ExportProgressForm();
        iconProgressForm.Text = "Pack Icons";
        var iconProgress = new Progress<IconPackProgress>(p => iconProgressForm.Report("Packing icons", p.Current, p.Total));
        iconProgressForm.Show(this);
        try
        {
            var packed = await _iconPackService.PackAsync(_workingSettings.EntityIconsPath!, iconsDb, iconProgress, CancellationToken.None);
            MessageBox.Show(this, $"Packed {packed} icons.", "Pack Icons", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Icon pack failed");
            MessageBox.Show(this, $"Icon pack failed: {ex.Message}", "Pack Icons", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            iconProgressForm.Close();
        }
    }

    private void UpdateIconSettingsUi()
    {
        var enabled = _chkEnableEntityIcons?.Checked == true;

        if (_txtEntityIconsPath is not null)
        {
            _txtEntityIconsPath.Enabled = enabled;
        }

        if (_btnBrowseEntityIconsPath is not null)
        {
            _btnBrowseEntityIconsPath.Enabled = enabled;
        }
    }

    private void btnBrowseEntityIconsPath_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select folder with icon files",
            ShowNewFolderButton = false
        };

        if (_txtEntityIconsPath is not null && Directory.Exists(_txtEntityIconsPath.Text))
        {
            dialog.SelectedPath = _txtEntityIconsPath.Text;
        }

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (_txtEntityIconsPath is not null)
        {
            _txtEntityIconsPath.Text = dialog.SelectedPath;
        }
    }
}
