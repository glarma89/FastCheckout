using Serilog;
using System.Text.Json;

namespace FastCheckout
{
    public partial class RFIDController : Form
    {
        private readonly ReaderController readerController = new();
        private readonly GlobalKeyboardHook keyboardHook = new();
        private ContextMenuStrip? trayContextMenu;
        private Keys inventoryToggleKey = Keys.S;

        public RFIDController()
        {
            InitializeComponent();

            try
            {
                string iconPath = @"C:\Users\user\Documents\Paytag\Logos\icon.ico";
                notifyIcon.Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Application;
            }
            catch (Exception ex)
            {
                notifyIcon.Icon = SystemIcons.Application;
                Log.Error($"Failed to load custom icon: {ex.Message}");
            }

            this.FormClosing += Form_FormClosing;
            InitializeTrayContextMenu();

            // Reader events -> UI. OmitTags fires on a worker thread, so marshal.
            readerController.ConnectionChanged += connected =>
                BeginInvoke(() => UpdateConnectionStatus(connected));
            readerController.InventoryStateChanged += running =>
                BeginInvoke(() => UpdateInventoryStatus(running));
            readerController.TagScanned += tagId =>
                BeginInvoke(() => AddScannedTag(tagId));

        }

        protected override void OnLoad(EventArgs e)
        {
            // Defer init until the form's window handle exists — InitializeReader()
            // can fire ConnectionChanged before OnLoad on a fast-connecting reader,
            // and the handlers above use BeginInvoke, which requires a handle.
            base.OnLoad(e);

            try { readerController.InitializeReader(); }
            catch (Exception ex) { Log.Error(LogFormatter.Exception(ex)); }

            try { ConfigureHotkey(); }
            catch (Exception ex) { Log.Error(LogFormatter.Exception(ex)); }
        }

        private void ConfigureHotkey()
        {
            inventoryToggleKey = LoadInventoryToggleKey();

            keyboardHook.KeyPressed += key =>
            {
                if (key != inventoryToggleKey)
                    return;

                BeginInvoke(() =>
                {
                    try
                    {
                        if (readerController.IsInventoryRunning)
                            readerController.StopInventory();
                        else
                            readerController.StartInventory();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(LogFormatter.Exception(ex));
                    }
                });
            };

            keyboardHook.Install();
        }

        private static Keys LoadInventoryToggleKey()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AppConfig.SettingsFilePath))
                    return Keys.S;

                string json = File.ReadAllText(AppConfig.SettingsFilePath);

                using JsonDocument document = JsonDocument.Parse(json);

                if (!document.RootElement.TryGetProperty("Hotkeys", out JsonElement hotkeys))
                    return Keys.S;

                if (!hotkeys.TryGetProperty("InventoryToggleKey", out JsonElement keyElement))
                    return Keys.S;

                string? keyName = keyElement.GetString();

                if (string.IsNullOrWhiteSpace(keyName))
                    return Keys.S;

                return Enum.TryParse(keyName, ignoreCase: true, out Keys parsedKey)
                    ? parsedKey
                    : Keys.S;
            }
            catch (Exception ex)
            {
                Log.Error(LogFormatter.Exception(ex));
                return Keys.S;
            }
        }

        private void UpdateConnectionStatus(bool connected)
        {
            connectionStatusLabel.Text = connected ? "Reader: Connected" : "Reader: Disconnected";
            connectionStatusLabel.ForeColor = connected ? Color.ForestGreen : Color.Firebrick;
        }

        private void UpdateInventoryStatus(bool running)
        {
            inventoryStatusLabel.Text = running ? "Inventory: Running" : "Inventory: Stopped";
            inventoryStatusLabel.ForeColor = running ? Color.ForestGreen : Color.DimGray;
        }

        private void AddScannedTag(string tagId)
        {
            tagsListBox.Items.Insert(0, $"{DateTime.Now:HH:mm:ss.fff}  {tagId}");
            if (tagsListBox.Items.Count > 200)
                tagsListBox.Items.RemoveAt(tagsListBox.Items.Count - 1);
            tagCountLabel.Text = $"Tags scanned: {tagsListBox.Items.Count}";
        }

        private void InitializeTrayContextMenu()
        {
            trayContextMenu = new ContextMenuStrip();

            var showMenuItem = new ToolStripMenuItem("Show RFID Controller");
            showMenuItem.Click += (_, _) => { Show(); WindowState = FormWindowState.Normal; Activate(); };
            trayContextMenu.Items.Add(showMenuItem);

            trayContextMenu.Items.Add(new ToolStripSeparator());

            var closeMenuItem = new ToolStripMenuItem("Close");
            closeMenuItem.Click += (_, _) => { Log.Information("Application closed via tray menu"); Application.Exit(); };
            trayContextMenu.Items.Add(closeMenuItem);

            notifyIcon.ContextMenuStrip = trayContextMenu;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (Visible) Hide();
            else { Show(); WindowState = FormWindowState.Normal; Activate(); }
        }

        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall ||
                e.CloseReason == CloseReason.WindowsShutDown)
            {
                try { keyboardHook.Dispose(); } catch (Exception ex) { Log.Error(LogFormatter.Exception(ex)); }
                try { readerController.ShutDown(); } catch (Exception ex) { Log.Error(LogFormatter.Exception(ex)); }

                trayContextMenu?.Dispose();
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}
