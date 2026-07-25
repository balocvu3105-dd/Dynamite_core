using Dynamite.API.Hubs;
using Dynamite.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Dynamite.API.Services;

public class SignalRSyncNotifier : ISyncNotifier
{
    private readonly IHubContext<SyncHub> _hubContext;

    public SignalRSyncNotifier(IHubContext<SyncHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyConfigUpdatedAsync(ulong guildId)
    {
        await _hubContext.Clients.All.SendAsync("ConfigUpdated", guildId);
    }

    public async Task NotifyModuleFaultedAsync(ulong guildId, string moduleName, string reason)
    {
        await _hubContext.Clients.All.SendAsync("ModuleFaulted", guildId, moduleName, reason);
    }
}
