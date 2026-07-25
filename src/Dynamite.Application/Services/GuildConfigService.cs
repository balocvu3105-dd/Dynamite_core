namespace Dynamite.Application.Services;

using Dynamite.Application.Interfaces;
using Dynamite.Core.Entities;
using Dynamite.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

public class GuildConfigService : IGuildConfigService
{
    private readonly IGuildConfigRepository _guildConfigRepo;
    private readonly ISyncNotifier _syncNotifier;
    private readonly ILogger<GuildConfigService> _logger;

    public GuildConfigService(
        IGuildConfigRepository guildConfigRepo,
        ISyncNotifier syncNotifier,
        ILogger<GuildConfigService> logger)
    {
        _guildConfigRepo = guildConfigRepo;
        _syncNotifier = syncNotifier;
        _logger = logger;
    }

    public async Task<GuildConfig> GetOrCreateConfigAsync(
        ulong guildId, string guildName, CancellationToken ct = default)
    {
        var config = await _guildConfigRepo.GetOrCreateAsync(guildId, guildName, ct);
        _logger.LogDebug("Loaded config for guild {GuildId}", guildId);
        return config;
    }

    public async Task UpdateConfigAsync(GuildConfig config, CancellationToken ct = default)
    {
        config.UpdatedAt = DateTime.UtcNow;
        await _guildConfigRepo.UpdateAsync(config, ct);
        await _guildConfigRepo.SaveChangesAsync(ct);
        
        // Notify SignalR clients that this guild's config was updated
        try
        {
            await _syncNotifier.NotifyConfigUpdatedAsync(config.GuildId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to broadcast config update for guild {GuildId}", config.GuildId);
        }
    }

    // Fix: accept guildName so GetOrCreateAsync stores the real name,
    // not a fallback of guildId.ToString() when the config doesn't exist yet.
    public async Task SetModLogChannelAsync(
        ulong guildId, string guildName, ulong channelId, CancellationToken ct = default)
    {
        var config = await _guildConfigRepo.GetOrCreateAsync(guildId, guildName, ct);
        config.ModLogChannelId = channelId;
        await UpdateConfigAsync(config, ct);
    }
}