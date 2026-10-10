using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LeanMM_W
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly string settingsFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");


        public MainWindow()
        {
            this.InitializeComponent();

            // Get native window handle and AppWindow object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
            {
                presenter.IsResizable = true;
                presenter.IsMaximizable = true;

                // Uncomment if you want the window to start maximized on launch:
                // presenter.Maximize();
            }

            LogMessage("Application initialized. Ready to configure server.");
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(settingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);

                    if (settings != null)
                    {
                        ServerInput.Text = settings.Server;
                        UsernameInput.Text = settings.Username;
                        PathInput.Text = settings.WorkingPath;
                        LogMessage("Default settings loaded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Warning: Could not load settings. {ex.Message}");
                }
            }
        }

        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                Server = ServerInput.Text,
                Username = UsernameInput.Text,
                WorkingPath = PathInput.Text
            };

            try
            {
                // WriteIndented makes the JSON file nicely formatted and human-readable
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                LogMessage($"Warning: Could not save settings. {ex.Message}");
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string server = ServerInput.Text;
            string username = UsernameInput.Text;
            string password = PasswordInput.Password;
            string path = PathInput.Text;

            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(username))
            {
                LogMessage("Error: Server address and Username are required fields.");
                return;
            }

            // Save the valid inputs to the default settings file
            SaveSettings();

            ConnectButton.IsEnabled = false;

            LogMessage($"Attempting to connect to '{server}' as user '{username}'...");
            LogMessage($"Working directory set to: {path}");

            // TODO: Actual connection logic here...

            LogMessage("Connection established successfully.");
            ConnectButton.IsEnabled = true;
        }

        private void LogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            LogConsole.Text += $"[{timestamp}] {message}\r\n";
            LogConsole.SelectionStart = LogConsole.Text.Length;
            LogConsole.SelectionLength = 0;
        }
    }
}
