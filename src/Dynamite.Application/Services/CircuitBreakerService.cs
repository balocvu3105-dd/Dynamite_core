using System.Collections.Concurrent;
using Dynamite.Application.Interfaces;
using Dynamite.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dynamite.Application.Services;

public class CircuitBreakerService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISyncNotifier _syncNotifier;
    private readonly ILogger<CircuitBreakerService> _logger;

    // ConcurrentDictionary: <GuildId_ModuleName, ErrorCount>
    private readonly ConcurrentDictionary<string, int> _errorCounts = new();

    public CircuitBreakerService(
        IServiceScopeFactory scopeFactory,
        ISyncNotifier syncNotifier,
        ILogger<CircuitBreakerService> logger)
    {
        _scopeFactory = scopeFactory;
        _syncNotifier = syncNotifier;
        _logger = logger;
    }

    public void ReportSuccess(ulong guildId, string moduleName)
    {
        var key = $"{guildId}_{moduleName}";
        if (_errorCounts.TryRemove(key, out _))
        {
            _logger.LogInformation("Circuit Breaker reset for Guild {GuildId}, Module {ModuleName}", guildId, moduleName);
        }
    }

    public async Task ReportErrorAsync(ulong guildId, string moduleName, Exception ex)
    {
        var key = $"{guildId}_{moduleName}";
        var currentCount = _errorCounts.AddOrUpdate(key, 1, (_, count) => count + 1);

        _logger.LogWarning(ex, "Module {ModuleName} failed in Guild {GuildId}. Error count: {Count}", moduleName, guildId, currentCount);

        if (currentCount >= 3)
        {
            // Tự động ngắt module
            _logger.LogError("Circuit Breaker TRIPPED for Guild {GuildId}, Module {ModuleName} after 3 consecutive failures. Disabling module...", guildId, moduleName);
            await TripCircuitAsync(guildId, moduleName, ex.Message);
            _errorCounts.TryRemove(key, out _); // reset
        }
    }

    private async Task TripCircuitAsync(ulong guildId, string moduleName, string reason)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var guildConfigService = scope.ServiceProvider.GetRequiredService<IGuildConfigService>();

            var config = await guildConfigService.GetOrCreateConfigAsync(guildId, "Unknown");
            
            bool changed = false;
            switch (moduleName.ToLowerInvariant())
            {
                case "welcome":
                    if (config.WelcomeEnabled) { config.WelcomeEnabled = false; changed = true; }
                    break;
                case "logging":
                    if (config.LoggingEnabled) { config.LoggingEnabled = false; changed = true; }
                    break;
                case "moderation":
                    if (config.ModerationEnabled) { config.ModerationEnabled = false; changed = true; }
                    break;
                case "autorole":
                    if (config.AutoRoleEnabled) { config.AutoRoleEnabled = false; changed = true; }
                    break;
            }

            if (changed)
            {
                var fault = new ModuleFault
                {
                    GuildConfigId = config.Id,
                    ModuleName = moduleName,
                    Reason = $"Circuit Breaker tripped: {reason}",
                    FaultedAt = DateTime.UtcNow
                };

                config.ModuleFaults.Add(fault);
                await guildConfigService.UpdateConfigAsync(config);
                
                // Notify via SignalR
                await _syncNotifier.NotifyModuleFaultedAsync(guildId, moduleName, fault.Reason);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to trip circuit breaker for Guild {GuildId}, Module {ModuleName}", guildId, moduleName);
        }
    }
}
