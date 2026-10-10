using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5065/desktopHub")
    .WithAutomaticReconnect()
    .Build();

connection.On("ReceiveCommand", (string message) =>
{
    Console.WriteLine($"Command received: {message}");
});

await connection.StartAsync();
string myComputerId = Environment.MachineName; // Use the machine name as the computer ID
await connection.InvokeAsync("RegisterComputer", myComputerId);

Console.ReadLine(); // Keep the console app running to listen for messages