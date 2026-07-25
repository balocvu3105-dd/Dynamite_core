using Dynamite.Application.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dynamite.Bot.Services;

public class BotSyncClient : IHostedService, ISyncNotifier
{
    private HubConnection? _hubConnection;
    private readonly ILogger<BotSyncClient> _logger;
    private readonly string _apiUrl;

    public BotSyncClient(IConfiguration configuration, ILogger<BotSyncClient> logger)
    {
        _logger = logger;
        
        // Cố gắng lấy từ biến môi trường hoặc cấu hình, fallback về localhost
        _apiUrl = configuration["ApiUrl"] ?? "http://dynamite_api:8080";
        if (_apiUrl == "http://dynamite_api:8080" && Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development")
        {
            _apiUrl = "http://localhost:5266";
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_apiUrl.TrimEnd('/')}/hubs/sync")
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.Closed += async (error) =>
        {
            _logger.LogWarning(error, "SignalR connection closed. Retrying...");
            await Task.Delay(new Random().Next(0, 5) * 1000, cancellationToken);
            try { await _hubConnection.StartAsync(cancellationToken); } catch { }
        };

        try
        {
            await _hubConnection.StartAsync(cancellationToken);
            _logger.LogInformation("SignalR BotSyncClient connected to {ApiUrl}", _apiUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not connect to SignalR SyncHub at startup. Will retry automatically.");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync(cancellationToken);
            await _hubConnection.DisposeAsync();
        }
    }

    public async Task NotifyConfigUpdatedAsync(ulong guildId)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("NotifyConfigUpdated", guildId);
        }
    }

    public async Task NotifyModuleFaultedAsync(ulong guildId, string moduleName, string reason)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("NotifyModuleFaulted", guildId, moduleName, reason);
        }
    }
}
