using Microsoft.AspNetCore.SignalR.Client;

namespace LeanMM_Wk
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Properties.Settings.Default.Server;
            textBoxUser.Text = Properties.Settings.Default.User;
            textBoxLocalFolder.Text = Properties.Settings.Default.LocalFolder;
        }

        private void buttonPickFolder_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            textBoxLocalFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Server = textBoxServer.Text;
            Properties.Settings.Default.User = textBoxUser.Text;
            Properties.Settings.Default.LocalFolder = textBoxLocalFolder.Text;
            Properties.Settings.Default.Save();

            var url = (new Uri(new Uri(textBoxServer.Text), "desktopHub")).AbsoluteUri;

            var connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            listBoxConsole.Items.Add($"Connecting to: {url}");

            connection.On<string>("ReceiveMessage", (message) =>
            {
                listBoxConsole.Invoke(new Action(() =>
                {
                    listBoxConsole.Items.Add($"Message from Blazor: {message}");
                }));
            });

            try
            {
                await connection.StartAsync();
                listBoxConsole.Items.Add("Connected successfully!");
            }
            catch(Exception ex)
            {
                listBoxConsole.Items.Add($"Connection failed: {ex.Message}");
                // If there is an inner exception, print it too
                if (ex.InnerException != null)
                {
                    listBoxConsole.Items.Add($"Inner exception: {ex.InnerException.Message}");
                }
                return;
            }

            string myComputerId = Environment.MachineName;
            await connection.InvokeAsync("RegisterComputer", myComputerId);
            await connection.InvokeAsync("SendMessageToServer", myComputerId, "Hello");
        }
    }
}
