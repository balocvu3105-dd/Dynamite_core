namespace Dynamite.Application.Interfaces;

public interface ISyncNotifier
{
    Task NotifyConfigUpdatedAsync(ulong guildId);
    Task NotifyModuleFaultedAsync(ulong guildId, string moduleName, string reason);
}
