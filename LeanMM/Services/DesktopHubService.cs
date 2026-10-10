namespace LeanMM.Services
{
    public class DesktopHubService
    {
        public Action<string, string>? OnMessageReceived;

        public void NotifyMessageReceived(string computerId, string message)
        {
            OnMessageReceived?.Invoke(computerId, message);
        }
    }
}
