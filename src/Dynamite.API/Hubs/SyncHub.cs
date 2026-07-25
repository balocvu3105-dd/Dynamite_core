using Microsoft.AspNetCore.SignalR;

namespace Dynamite.API.Hubs;

public class SyncHub : Hub
{
    // The Bot will call this method when it changes configuration
    public async Task NotifyConfigUpdated(ulong guildId)
    {
        // Broadcast to all connected clients that this guild's config was updated
        await Clients.All.SendAsync("ConfigUpdated", guildId);
    }

    // The Bot will call this method when a module crashes
    public async Task NotifyModuleFaulted(ulong guildId, string moduleName, string reason)
    {
        // Broadcast the fault to all clients
        await Clients.All.SendAsync("ModuleFaulted", guildId, moduleName, reason);
    }
}
