using LeanMM.Services;
using Microsoft.AspNetCore.SignalR;

namespace LeanMM.Hubs
{
    public class DesktopHub : Hub
    {
        private readonly DesktopHubService _hubService;

        public DesktopHub(DesktopHubService hubService)
        {
            _hubService = hubService;
        }

        public async Task RegisterComputer(string computerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, computerId);
        }

        public async Task SendMessageToClient(string computerId, string message)
        {
            await Clients.Group(computerId).SendAsync("ReceiveCommand", message);
        }

        public async Task SendMessageToServer(string computerId, string message)
        {
            _hubService.NotifyMessageReceived(computerId, message);
        }
    }
}
